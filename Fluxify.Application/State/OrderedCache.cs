// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Concurrent;
using Fluxify.Application.Entities;
using Fluxify.Application.Entities.Channels;
using Fluxify.Core.Types;

namespace Fluxify.Application.State;

/// <summary>
/// Cache with FIFO purge strategy
/// </summary>
/// <param name="mapper"></param>
/// <param name="maxCacheSize"></param>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TMapper"></typeparam>
internal sealed class OrderedCache<TData, TMapper>(TMapper mapper, long maxCacheSize) : ICache<TData>
    where TData : class, IEntity, ICloneable<TData>
    where TMapper : IUpdateEntity<TData>
{
    private readonly ConcurrentDictionary<Snowflake, CacheRef<TData>> _dataContainer = new();
    private ConcurrentQueue<Snowflake> _keyOrder = new();
    private readonly ReaderWriterLockSlim _queueReplaceLock = new();
    private readonly ResourceTransactions<CacheRef<TData>> _transactions = new();

    public bool IsCached(Snowflake id) => _dataContainer.ContainsKey(id);

    public CacheRef<TData> GetCachedOrDefault(Snowflake id)
        => _dataContainer.TryGetValue(id, out var result)
            ? result
            : new CacheRef<TData>(id, null);

    public IReadOnlyCollection<CacheRef<TData>> GetAllCached() =>
        (IReadOnlyCollection<CacheRef<TData>>)_dataContainer.Values;

    public IReadOnlyList<CacheRef<TData>>? GetPaged(Snowflake? key, Direction direction, int pageSize)
    {
        pageSize = Math.Clamp(pageSize, 0, 100);

        if (direction == Direction.After && key == null)
        {
            return null;
        }

        var byDirection = direction switch
        {
            Direction.Before => _keyOrder.Reverse(),
            Direction.After => _keyOrder,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
        var start = key != null ? byDirection.SkipWhile(x => x != key).Skip(1) : byDirection;

        var page = start.Take(pageSize)
            .Select(id => _dataContainer[id])
            .ToArray();

        return page.Length <= 0 ? null : page.AsReadOnly();
    }

    public IReadOnlyCollection<CacheRef<TData>>? GetAround(Snowflake key, int count)
    {
        var snowflakes = _keyOrder.ToArray();
        var entry = snowflakes.BinarySearch(key, new TimeSnowflakeComparer());
        if (entry < 0)
        {
            return null;
        }

        var start = Math.Max(0, entry - count);
        var end = Math.Min(snowflakes.Length, entry + count);

        return snowflakes[start..end]
            .Select(id => _dataContainer[id])
            .ToList()
            .AsReadOnly();
    }

    private class TimeSnowflakeComparer : IComparer<Snowflake>
    {
        public int Compare(Snowflake x, Snowflake y) => x.FluxerEpochMs.CompareTo(y.FluxerEpochMs);
    }

    public async Task<CacheRef<TData>> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TData>> factory,
        bool bypassCache = false)
    {
        if (!bypassCache && _dataContainer.TryGetValue(id, out var data))
        {
            return data;
        }

        return await _transactions.BeginAsync(id, async () => UpdateOrCreate(await factory(id)));
    }

    public ICollection<CacheRef<TData>> UpdateOrCreate(ICollection<TData> data)
    {
        _queueReplaceLock.EnterUpgradeableReadLock();
        if (!_keyOrder.TryPeek(out var lastElement)
            || data.All(x => x.Id < lastElement))
        {
            _queueReplaceLock.ExitUpgradeableReadLock();

            return data.Select(UpdateOrCreate).ToList();
        }

        _queueReplaceLock.EnterWriteLock();
        _keyOrder = new ConcurrentQueue<Snowflake>([.._keyOrder, ..MassInsert(data)]);
        _queueReplaceLock.ExitWriteLock();

        while (_keyOrder.Count > maxCacheSize)
        {
            _keyOrder.TryDequeue(out var id);
            _dataContainer.TryRemove(id, out _);
        }

        _queueReplaceLock.ExitUpgradeableReadLock();

        return data.Select(d => _dataContainer[d.Id]).ToList();
    }

    public IEnumerable<Snowflake> MassInsert(IEnumerable<TData> data)
    {
        foreach (var entity in data)
        {
            _dataContainer.AddOrUpdate(entity.Id,
                id => new CacheRef<TData>(id, entity),
                (_, existing) =>
                {
                    if (existing.Value?.Clone() is {} cloned)
                    {
                        mapper.UpdateEntity(cloned, entity);
                    }
                    else
                    {
                        cloned = entity;
                    }
                
                    existing.Swap(cloned);
                    return existing;
                });

            yield return entity.Id;
        }
    }

    public CacheRef<TData> UpdateOrCreate(TData data)
        => _dataContainer.AddOrUpdate(data.Id,
            key =>
            {
                _queueReplaceLock.EnterReadLock();
                try
                {
                    if (_keyOrder.Count >= maxCacheSize)
                    {
                        _keyOrder.TryDequeue(out var id);
                        _dataContainer.TryRemove(id, out _);
                    }

                    _keyOrder.Enqueue(key);
                }
                finally
                {
                    _queueReplaceLock.ExitReadLock();
                }

                return new CacheRef<TData>(key, data);
            },
            (_, existing) =>
            {
                if (existing.Value?.Clone() is {} cloned)
                {
                    mapper.UpdateEntity(cloned, data);
                }
                else
                {
                    cloned = data;
                }
                
                existing.Swap(cloned);
                return existing;
            });

    public bool TryUpdate(Snowflake key, Action<TData> update, out CacheRef<TData> updated)
    {
        if (_dataContainer.TryGetValue(key, out var data)
            && data.Value?.Clone() is {} cloned)
        {
            update(cloned);
            data.Swap(cloned);
            updated = data;

            return true;
        }
        
        updated = new CacheRef<TData>(key, null);
        return false;
    }

    public bool Remove(Snowflake id, out CacheRef<TData> message)
    {
        if (!_dataContainer.TryRemove(id, out message))
        {
            return false;
        }

        _queueReplaceLock.EnterWriteLock();
        // rebuild queue 
        try
        {
            _keyOrder = new ConcurrentQueue<Snowflake>(_keyOrder.Where(key => key != id));
        }
        finally
        {
            _queueReplaceLock.ExitWriteLock();
        }

        return true;
    }


    public void RemoveAll(Snowflake[] id, out CacheRef<TData>[] removedMessages)
    {
        _queueReplaceLock.EnterWriteLock();
        // rebuild queue 
        try
        {
            removedMessages = _dataContainer
                .Select(key 
                    => _dataContainer.TryRemove(key.Key, out var message) 
                        ? message
                        : new CacheRef<TData>(key.Key, null))
                .ToArray();

            var hashSet = id.ToHashSet();
            _keyOrder = new ConcurrentQueue<Snowflake>(_keyOrder.Where(key => !hashSet.Contains(key)));
        }
        finally
        {
            _queueReplaceLock.ExitWriteLock();
        }
    }

    public void Clear()
    {
        _dataContainer.Clear();
        _queueReplaceLock.EnterReadLock();

        try
        {
            _keyOrder.Clear();
        }
        finally
        {
            _queueReplaceLock.ExitReadLock();
        }
    }
}