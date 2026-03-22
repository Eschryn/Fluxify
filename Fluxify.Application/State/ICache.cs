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
using Fluxify.Core.Types;

namespace Fluxify.Application.State;

internal interface ICache<TData> where TData : class, IEntity
{
    bool IsCached(Snowflake id);
    T? GetCachedOrDefault<T>(Snowflake id) where T : TData;
    ICollection<TData> GetAllCached();

    Task<TData> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TData>> factory,
        bool bypassCache = false);

    TData UpdateOrCreate(TData data);
    void Remove(Snowflake id);
    void Clear();

    public static ICache<TData> CreateLimited<TMapper>(long limit, TMapper mapper) where TMapper : IUpdateEntity<TData>
        => limit switch
        {
            long.MaxValue => new PermanentCache<TData, TMapper>(mapper),
            > 0 => new LimitedCache<TData, TMapper>(mapper, limit),
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