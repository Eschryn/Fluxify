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
using Fluxify.Application.Model.Channel;
using Fluxify.Application.Model.Guild;
using Fluxify.Application.State;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Users.Settings.Security;
using Fluxify.Rest;

namespace Fluxify.Application.Repositories;

internal sealed class GuildRepository(RestClient client, GuildMapper mapper, CacheConfig config)
{
    internal readonly ICache<Guild, GuildResponse> Cache = ICache<Guild, GuildResponse>.CreateLru(config.GuildCacheSize, mapper);

    public Task<CacheRef<Guild>> GetAsync(Snowflake id, bool bypassCache = false)
        => Cache.GetOrCreateAsync(id, GetGuildRestAsync, bypassCache);

    internal CacheRef<Guild> Insert(GuildResponse response)
        => Cache.UpdateOrCreate(response.Id, response);


    private async Task<GuildResponse> GetGuildRestAsync(Snowflake id)
        => await client.Guilds[id].GetAsync() ?? throw new Exception($"Couldn't get user with id {id}");

    internal void Reset() => Cache.Clear();

    public async Task<Guild> UpdateAsync(Guild guild, SudoVerificationSchema verificationSchema,
        Action<GuildProperties> update, CancellationToken cancellationToken)
    {
        var guildProperties = mapper.ToProperties(guild)
            .Configure(update);

        var request = mapper.ToUpdateRequest(
            guildProperties,
            verificationSchema.MfaCode,
            verificationSchema.MfaMethod,
            verificationSchema.Password,
            verificationSchema.WebauthnChallenge,
            verificationSchema.WebauthnResponse
        );
        
        var result = await guild.RequestBuilder.UpdateAsync(request, cancellationToken)
                     ?? throw new Exception("Guild was not updated");

        return Cache.UpdateOrCreate(result.Id, result).Value!;
    }
}