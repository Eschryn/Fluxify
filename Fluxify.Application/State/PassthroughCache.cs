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

using Fluxify.Application.Entities;

namespace Fluxify.Application.State;

internal sealed class PassthroughCache<TData, TDto, TMapper>(TMapper mapper) : ICache<TData, TDto>
    where TData : class, IEntity, ICloneable<TData>
    where TMapper : IUpdateEntity<TData, TDto>, ICreateEntity<TData, TDto>
{
    public bool IsCached(Snowflake id) => false;
    public CacheRef<TData> GetCachedOrDefault(Snowflake id) => new(id, null);
    public IReadOnlyCollection<CacheRef<TData>> GetAllCached() => [];

    public async Task<CacheRef<TData>> GetOrCreateAsync(
        Snowflake id,
        Func<Snowflake, Task<TDto>> factory,
        bool bypassCache = false
    ) => new(id, mapper.MapFromResponse(await factory(id)));

    public CacheRef<TData> UpdateOrCreate(Snowflake key, TDto dto) => new(key, mapper.MapFromResponse(dto));
    
    public bool TryUpdate(Snowflake key, Action<TData> update, out CacheRef<TData> updated)
    {
        updated = new CacheRef<TData>(key, null);
        return false;
    }

    public bool Remove(Snowflake id, out CacheRef<TData> data)
    {
        data = new CacheRef<TData>(id, null);
        return false;
    }
    public void Clear() { }
}