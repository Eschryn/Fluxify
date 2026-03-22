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

using Fluxify.Application.Common;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.State;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Users;
using Fluxify.Rest;

namespace Fluxify.Application.Repositories;

public sealed class GuildRepository(RestClient client, GuildMapper mapper, CacheConfig config)
{
    internal readonly ICache<Guild> Cache = ICache<Guild>.CreateLru(config.GuildCacheSize, mapper);
   
    public Task<Guild> GetAsync(Snowflake id, bool bypassCache = false) 
        => Cache.GetOrCreateAsync(id, GetGuildRestAsync, bypassCache);

    internal Guild Insert(GuildResponse response)
    {
        var mapped = mapper.MapCached(response);
        Cache.UpdateOrCreate(mapped);
        return mapped;
    }


    private async Task<Guild> GetGuildRestAsync(Snowflake id) 
        => await client.Guilds[id].GetAsync() is {} guild 
            ? await mapper.MapAsync(guild) 
            : throw new Exception($"Couldn't get user with id {id}");
   
    internal void Reset() => Cache.Clear();
}