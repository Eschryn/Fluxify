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
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Fluxify.Application.Entities;

namespace Fluxify.Application.State;

internal record struct LruCacheEntry<T>(
    CacheRef<T> Data,
    LinkedListNode<Snowflake> Node
) where T : class, IEntity, ICloneable<T>
{
    public readonly LinkedListNode<Snowflake> Node = Node;
}

// Cache with LRU purge strategy
internal sealed class LruCache<TData, TDto, TMapper>(TMapper mapper, long maxCacheSize) : ICache<TData, TDto>
    where TData : class, IEntity, ICloneable<TData>
    where TMapper : IUpdateEntity<TData, TDto>, ICreateEntity<TData, TDto>
{
    private readonly ConcurrentDictionary<Snowflake, LruCacheEntry<TData>> _dataContainer = new();
    private readonly LinkedList<Snowflake> _keyOrder = new();
    private readonly ResourceTransactions<CacheRef<TData>> _transactions = new();

    public bool IsCached(Snowflake id) => _dataContainer.ContainsKey(id);

    public CacheRef<TData> GetCachedOrDefault(Snowflake id)
        => TryGet(id, out var data) ? data : new CacheRef<TData>(id, null);

    public CacheRef<TData> GetCachedOrCreateEmpty(Snowflake id)
        => _dataContainer.GetOrAdd(
            id,
            key => new LruCacheEntry<TData>(
                new CacheRef<TData>(key, null),
                InsertNewEntry(key)
            )
        ).Data;

    private bool TryGet(Snowflake key, out CacheRef<TData> data)
    {
        if (_dataContainer.TryGetValue(key, out var container))
        {
            data = container.Data;
            RenewEntry(container.Node);
            return true;
        }

        data = new CacheRef<TData>(key, null);
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RenewEntry(LinkedListNode<Snowflake> entry)
    {
        lock (_keyOrder)
        {
            _keyOrder.Remove(entry);
            _keyOrder.AddFirst(entry);
        }
    }

    public IReadOnlyCollection<CacheRef<TData>> GetAllCached()
        => _dataContainer.Values.Select(e => e.Data).ToImmutableArray().AsReadOnly();

    public async Task<CacheRef<TData>> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TDto>> factory,
        bool bypassCache = false)
    {
        if (!bypassCache && TryGet(id, out var data))
        {
            return data;
        }

        return await _transactions.BeginAsync(id, async () => UpdateOrCreate(id, await factory(id)));
    }

    public CacheRef<TData> UpdateOrCreate(Snowflake key, TDto dto)
        => _dataContainer.AddOrUpdate(key,
            _ => new LruCacheEntry<TData>(
                new CacheRef<TData>(key, mapper.MapFromResponse(dto)),
                InsertNewEntry(key)),
            (_, existing) =>
            {
                RenewEntry(existing.Node);
                if (existing.Data.Value?.Clone() is { } cloned)
                {
                    mapper.UpdateEntity(cloned, dto);
                }
                else
                {
                    cloned = mapper.MapFromResponse(dto);
                }

                existing.Data.Swap(cloned);
                return existing;
            }).Data;

    public bool TryUpdate(Snowflake key, Action<TData> update, out CacheRef<TData> updated)
    {
        if (_dataContainer.TryGetValue(key, out var entry)
            && entry.Data.Value?.Clone() is { } cloned)
        {
            update(cloned);
            RenewEntry(entry.Node);
            entry.Data.Swap(cloned);
            updated = entry.Data;

            return true;
        }

        updated = new CacheRef<TData>(key, null);
        return false;
    }

    private LinkedListNode<Snowflake> InsertNewEntry(Snowflake key)
    {
        var newEntry = new LinkedListNode<Snowflake>(key);

        lock (_keyOrder)
        {
            _keyOrder.AddFirst(newEntry);

            if (_keyOrder.Count < maxCacheSize || _keyOrder.First is not { } entry)
            {
                return newEntry;
            }

            key = entry.Value;
            _keyOrder.RemoveLast();
        }

        _dataContainer.TryRemove(key, out _);
        return newEntry;
    }

    public bool Remove(Snowflake id, out CacheRef<TData> data)
    {
        var result = _dataContainer.TryRemove(id, out var entry);
        lock (_keyOrder)
        {
            data = entry.Data;
            _keyOrder.Remove(entry.Node);
        }

        return result;
    }

    public void Clear()
    {
        _dataContainer.Clear();
        lock (_keyOrder)
        {
            _keyOrder.Clear();
        }
    }
}