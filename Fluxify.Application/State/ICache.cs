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
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;

namespace Fluxify.Application.State;

internal interface ICache<TData> where TData : class, IEntity, ICloneable<TData>
{
    bool IsCached(Snowflake id);
    CacheRef<TData> GetCachedOrDefault(Snowflake id);
    IReadOnlyCollection<CacheRef<TData>> GetAllCached();

    Task<CacheRef<TData>> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TData>> factory,
        bool bypassCache = false);

    CacheRef<TData> UpdateOrCreate(TData data);
    bool TryUpdate(Snowflake key, Action<TData> update, out CacheRef<TData> updated);
    bool Remove(Snowflake id, out CacheRef<TData> data);
    void Clear();

    public static ICache<TData> CreateOrdered<TMapper>(long limit, TMapper mapper) where TMapper : IUpdateEntity<TData>
        => limit switch
        {
            > 0 => new OrderedCache<TData, TMapper>(mapper, limit),
            _ => new PassthroughCache<TData>()
        };

    public static ICache<TData> CreateLru<TMapper>(long limit, TMapper mapper) where TMapper : IUpdateEntity<TData>
        => limit switch
        {
            long.MaxValue => new PermanentCache<TData, TMapper>(mapper),
            > 0 => new LruCache<TData,TMapper>(mapper, limit),
            _ => new PassthroughCache<TData>()
        };
}