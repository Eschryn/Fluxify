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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Fluxify.Application.Entities;
using Fluxify.Core.Types;

namespace Fluxify.Application.State;

/// <summary>
/// Cache with LRU purge strategy
/// </summary>
/// <param name="mapper"></param>
/// <param name="maxCacheSize"></param>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TMapper"></typeparam>
internal sealed class LruCache<TData, TMapper>(TMapper mapper, long maxCacheSize) : ICache<TData>
    where TData : class, IEntity
    where TMapper : IUpdateEntity<TData>
{
    private readonly ConcurrentDictionary<Snowflake, TData> _dataContainer = new();
    private readonly ConcurrentDictionary<Snowflake, LinkedListNode<Snowflake>> _keyContainer = new();
    private readonly LinkedList<Snowflake> _keyOrder = new();
    private readonly ResourceTransactions<TData> _transactions = new();

    public bool IsCached(Snowflake id) => _dataContainer.ContainsKey(id);

    public T? GetCachedOrDefault<T>(Snowflake id) where T : TData
        => TryGet(id, out var data) ? (T?)data : default;

    private bool TryGet(Snowflake key, [NotNullWhen(true)] out TData? data)
    {
        if (_dataContainer.TryGetValue(key, out data))
        {
            RenewEntry(key);
            return true;
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RenewEntry(Snowflake key)
    {
        lock (_keyOrder)
        {
            var entry = _keyContainer[key];
            _keyOrder.Remove(entry);
            _keyOrder.AddFirst(entry);
        }
    }

    public ICollection<TData> GetAllCached() => _dataContainer.Values;

    public async Task<TData> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TData>> factory,
        bool bypassCache = false)
    {
        if (!bypassCache && TryGet(id, out var data))
        {
            return data;
        }

        return await _transactions.BeginAsync(id, async () => UpdateOrCreate(await factory(id)));
    }

    public TData UpdateOrCreate(TData data)
        => _dataContainer.AddOrUpdate(data.Id,
            key =>
            {
                InsertNewEntry(key);
                return data;
            },
            (key, existing) =>
            {
                RenewEntry(key);
                mapper.UpdateEntity(existing, data);

                return existing;
            });

    private void InsertNewEntry(Snowflake key)
    {
        lock (_keyOrder)
        {
            var newEntry = new LinkedListNode<Snowflake>(key);
            _keyContainer.TryAdd(key, newEntry);
            _keyOrder.AddFirst(newEntry);

            if (_keyOrder.Count < maxCacheSize || _keyOrder.First is not { } entry)
            {
                return;
            }

            key = entry.Value;
            _keyOrder.RemoveLast();
            _keyContainer.TryRemove(key, out _);
        }

        _dataContainer.TryRemove(key, out _);
    }

    public void Remove(Snowflake id)
    {
        _dataContainer.TryRemove(id, out _);
        lock (_keyOrder)
        {
            if (_keyContainer.TryRemove(id, out var entryNode))
            {
                _keyOrder.Remove(entryNode);
            }
        }
    }

    public void Clear()
    {
        _dataContainer.Clear();
        lock (_keyOrder)
        {
            _keyContainer.Clear();
            _keyOrder.Clear();
        }
    }
}