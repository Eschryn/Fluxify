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

public class PermanentCache<TData, TMapper>(TMapper mapper)
    where TData : class, IEntity
    where TMapper : IUpdateEntity<TData>
{
    private readonly ConcurrentDictionary<Snowflake, TData> _dataContainer = new();
    private readonly ResourceLock _lock = new();

    public bool IsCached(Snowflake id) =>  _dataContainer.ContainsKey(id);
    public T? GetCachedOrDefault<T>(Snowflake id) where T : TData => (T?)_dataContainer.GetValueOrDefault(id);
    
    public async Task<TData> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TData>> factory, bool bypassCache = false)
    {
        if (bypassCache)
        {
            var entity = await factory(id);
            _dataContainer.TryAdd(id, entity);
            return entity;
        }
        
        if (_dataContainer.TryGetValue(id, out var data))
        {
            return data;
        }

        using var _ = await _lock.LockAsync(id);

        // we need to check again because maybe we got data from another thread
        if (_dataContainer.TryGetValue(id, out data))
        {
            return data;
        }

        return _dataContainer[id] = await factory(id);
    }

    public TData UpdateOrCreate(TData data)
        => _dataContainer.AddOrUpdate(data.Id,
            _ => data,
            (_, existing) => mapper.UpdateEntity(existing, data));
    
    public void Remove(Snowflake id) => _dataContainer.TryRemove(id, out _);
    public void Clear() => _dataContainer.Clear();
}