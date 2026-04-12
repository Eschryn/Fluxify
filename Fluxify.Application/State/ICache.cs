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

internal interface ICache<TData, TDto> 
    where TData : class, IEntity, ICloneable<TData>
{
    bool IsCached(Snowflake id);
    CacheRef<TData> GetCachedOrDefault(Snowflake id);
    CacheRef<TData> GetCachedOrCreateEmpty(Snowflake id);
    IReadOnlyCollection<CacheRef<TData>> GetAllCached();


    public Task<CacheRef<TData>> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TDto>> factory,
        bool bypassCache = false);

    CacheRef<TData> UpdateOrCreate(Snowflake key, TDto dto);
    bool TryUpdate(Snowflake key, Action<TData> update, out CacheRef<TData> updated);
    bool Remove(Snowflake id, out CacheRef<TData> data);
    void Clear();

    public static ICache<TData, TDto> CreateLru<TMapper>(long limit, TMapper mapper) where TMapper : IUpdateEntity<TData, TDto>, ICreateEntity<TData, TDto> => limit switch
        {
            long.MaxValue => new PermanentCache<TData, TDto, TMapper>(mapper),
            > 0 => new LruCache<TData, TDto, TMapper>(mapper, limit),
            _ => new PassthroughCache<TData, TDto, TMapper>(mapper)
        };
}