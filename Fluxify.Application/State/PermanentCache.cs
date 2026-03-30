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
using Fluxify.Application.Entities;
using Fluxify.Core.Types;

namespace Fluxify.Application.State;

/// <summary>
/// Cache with no purge strategy
/// </summary>
/// <param name="mapper"></param>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TMapper"></typeparam>
internal sealed class PermanentCache<TData, TMapper>(TMapper mapper) : ICache<TData>
    where TData : class, IEntity
    where TMapper : IUpdateEntity<TData>
{
    private readonly ConcurrentDictionary<Snowflake, TData> _dataContainer = new();
    private readonly ResourceTransactions<TData> _transactions = new();

    public bool IsCached(Snowflake id) => _dataContainer.ContainsKey(id);
    public T? GetCachedOrDefault<T>(Snowflake id) where T : TData => (T?)_dataContainer.GetValueOrDefault(id);

    public IReadOnlyCollection<TData> GetAllCached() => (IReadOnlyCollection<TData>)_dataContainer.Values;

    public async Task<TData> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TData>> factory,
        bool bypassCache = false)
    {
        if (!bypassCache && _dataContainer.TryGetValue(id, out var data))
        {
            return data;
        }

        return await _transactions.BeginAsync(id, async () => UpdateOrCreate(await factory(id)));
    }
    
    public bool TryUpdate(Snowflake key, Action<TData> update, [NotNullWhen(true)] out TData? updated)
    {
        if (_dataContainer.TryGetValue(key, out var data))
        {
            updated = data;
            update(updated);
            
            return _dataContainer.TryUpdate(key, updated, data);
        }
        
        updated = null;
        return false;
    }
    
    public TData UpdateOrCreate(TData data)
        => _dataContainer.AddOrUpdate(data.Id,
            _ => data,
            (_, existing) =>
            {
                mapper.UpdateEntity(existing, data);

                return existing;
            });

    public bool Remove(Snowflake id, [NotNullWhen(true)] out TData? data) => _dataContainer.TryRemove(id, out data);
    public void Clear() => _dataContainer.Clear();
}