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
using Fluxify.Core.Types;

namespace Fluxify.Application.State;

/// <summary>
/// Cache with FIFO purge strategy
/// </summary>
/// <param name="mapper"></param>
/// <param name="maxCacheSize"></param>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TMapper"></typeparam>
internal sealed class LimitedCache<TData, TMapper>(TMapper mapper, long maxCacheSize) : ICache<TData>
    where TData : class, IEntity
    where TMapper : IUpdateEntity<TData>
{
    private readonly ConcurrentDictionary<Snowflake, TData> _dataContainer = new();
    private ConcurrentQueue<Snowflake> _keyOrder = new();
    private readonly ReaderWriterLockSlim _queueReplaceLock = new();
    private readonly ResourceTransactions<TData> _transactions = new();

    public bool IsCached(Snowflake id) => _dataContainer.ContainsKey(id);
    public T? GetCachedOrDefault<T>(Snowflake id) where T : TData => (T?)_dataContainer.GetValueOrDefault(id);

    public ICollection<TData> GetAllCached() => _dataContainer.Values;

    public async Task<TData> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TData>> factory,
        bool bypassCache = false)
    {
        if (!bypassCache && _dataContainer.TryGetValue(id, out var data))
        {
            return data;
        }

        return await _transactions.BeginAsync(id, async () => UpdateOrCreate(await factory(id)));
    }

    public TData UpdateOrCreate(TData data)
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
                
                return data;
            },
            (_, existing) =>
            {
                mapper.UpdateEntity(existing, data);

                return existing;
            });

    public void Remove(Snowflake id)
    {
        _dataContainer.TryRemove(id, out _);
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