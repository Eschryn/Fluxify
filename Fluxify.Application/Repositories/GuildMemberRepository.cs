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
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Rest;

namespace Fluxify.Application.Repositories;

internal sealed class GuildMemberRepository(
    Guild guild,
    RestClient client,
    UserMapper mapper,
    UserRepository userRepository,
    GuildRepository guildRepository,
    CacheConfig config
)
{
    internal UserRepository UserRepository { get; } = userRepository;
    internal ICache<IGuildMember> Cache = ICache<IGuildMember>.CreateLru(config.GuildUserCacheSize, mapper);

    public async Task<CacheRef<IGuildMember>> GetAsync(Snowflake roleId) => await Cache.GetOrCreateAsync(roleId, Factory);

    private async Task<IGuildMember> Factory(Snowflake memberId)
    {
        var guildMemberResponse = await client.Guilds[guild.Id].Members[memberId].GetAsync();

        return guildMemberResponse is null
            ? throw new Exception("Could not get guild member")
            : Insert(
                guildMemberResponse,
                await guildRepository.GetAsync(guild.Id),
                await UserRepository.GetAsync(memberId)
            ).Value!;
    }

    internal CacheRef<IGuildMember> Insert(GuildMemberResponse member, 
        CacheRef<Guild> guildRef,
        CacheRef<GlobalUser> userRef)
        => Cache.UpdateOrCreate(mapper.Map(member, userRef, guildRef));

    internal void Delete(Snowflake memberId)
    {
        Cache.Remove(memberId, out _);
    }
}