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

internal sealed class PassthroughCache<TData> : ICache<TData>
    where TData : class, IEntity
{
    public bool IsCached(Snowflake id) => false;
    public T? GetCachedOrDefault<T>(Snowflake id) where T : TData => default;
    public ICollection<TData> GetAllCached() => [];

    public Task<TData> GetOrCreateAsync(Snowflake id, Func<Snowflake, Task<TData>> factory, bool bypassCache = false)
        => factory(id);

    public TData UpdateOrCreate(TData data) => data;
    public void Remove(Snowflake id) { }
    public void Clear() { }
}