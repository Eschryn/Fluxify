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

using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.State;
using Fluxify.Dto.Guilds.Roles;
using Fluxify.Rest;

namespace Fluxify.Application.Repositories;

internal sealed class RoleRepository(Snowflake guildId, RestClient client, RoleMapper mapper, GuildRepository guildRepository)
{
    internal readonly PermanentCache<IRole, RoleInsert, RoleMapper> Cache = new(mapper);

    public async Task<CacheRef<IRole>> GetAsync(Snowflake roleId) => await Cache.GetOrCreateAsync(roleId, Factory);

    private async Task<IRole> Factory(Snowflake arg)
    {
        IRole? role = null;
        var guildRoleResponses = await client.Guilds[guildId].Roles.ListAsync() ?? [];
        foreach (var guildRoleResponse in guildRoleResponses)
        {
            if (guildRoleResponse.Id == arg)
            {
                role = mapper.MapFromResponse(new RoleInsert(guildRoleResponse, guildRepository.Cache.GetCachedOrDefault(guildId)));
            }
            
            Insert(guildRoleResponse, await guildRepository.GetAsync(guildId));
        }
        
        return role ?? throw new Exception($"Role with id {arg} not found");
    }

    internal void Insert(GuildRoleResponse role, CacheRef<Guild> guildRef)
    { 
        Cache.UpdateOrCreate(mapper.MapFromResponse(new RoleInsert(role, guildRef)));
    }

    internal void Delete(Snowflake argRoleId)
    {
        Cache.Remove(argRoleId, out _);
    }
}