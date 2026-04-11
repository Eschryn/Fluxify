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

namespace Fluxify.Application.State;

/// <summary>
/// Cache with no purge strategy
/// </summary>
/// <param name="mapper"></param>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TMapper"></typeparam>
/// <typeparam name="TDto"></typeparam>
internal sealed class PermanentCache<TData, TDto, TMapper>(TMapper mapper) : ICache<TData, TDto>
    where TData : class, IEntity, ICloneable<TData>
    where TMapper : IUpdateEntity<TData, TDto>, ICreateEntity<TData, TDto>
{
    private readonly ConcurrentDictionary<Snowflake, CacheRef<TData>> _dataContainer = new();
    private readonly ResourceTransactions<CacheRef<TData>> _transactions = new();

    public bool IsCached(Snowflake id) => _dataContainer.ContainsKey(id);
    public CacheRef<TData> GetCachedOrDefault(Snowflake id) 
        => _dataContainer.GetValueOrDefault(id) is { Id: var key} @ref 
           && key == id 
            ? @ref 
            : new CacheRef<TData>(id, null);

    public IReadOnlyCollection<CacheRef<TData>> GetAllCached() => (IReadOnlyCollection<CacheRef<TData>>)_dataContainer.Values;
    public IReadOnlyDictionary<Snowflake, CacheRef<TData>> GetDictionary() => _dataContainer;

    public async Task<CacheRef<TData>> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TDto>> factory,
        bool bypassCache = false)
    {
        if (!bypassCache && _dataContainer.TryGetValue(id, out var data))
        {
            return data;
        }

        return await _transactions.BeginAsync(id, async () => UpdateOrCreate(id, await factory(id)));
    }
    
    public bool TryUpdate(Snowflake key, Action<TData> update, out CacheRef<TData> updated)
    {
        if (_dataContainer.TryGetValue(key, out var data)
            && data.Value?.Clone() is {} cloned)
        {
            update(cloned);
            data.Swap(cloned);
            updated = data;

            _dataContainer.TryUpdate(key, data, data);
            return true;
        }
        
        updated = new CacheRef<TData>(key, null);
        return false;
    }
    
    public CacheRef<TData> UpdateOrCreate(Snowflake key, TDto data)
        => _dataContainer.AddOrUpdate(key,
            id => new CacheRef<TData>(id, mapper.MapFromResponse(data)),
            (_, existing) =>
            {
                if (existing.Value?.Clone() is {} cloned)
                {
                    mapper.UpdateEntity(cloned, data);
                }
                else
                {
                    cloned = mapper.MapFromResponse(data);
                }
                
                existing.Swap(cloned);
                return existing;
            });

    public bool Remove(Snowflake id, out CacheRef<TData> data)
    {
        if (_dataContainer.TryRemove(id, out data))
        {
            return true;
        }
        
        data = new CacheRef<TData>(id, null);
        return false;
    }

    public void Clear() => _dataContainer.Clear();
}