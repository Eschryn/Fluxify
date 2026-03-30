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

using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Dto.Guilds;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Guilds;

[Mapper]
public partial class GuildMapper(FluxerApplication app) : IUpdateEntity<Guild>
{
    public async Task<Guild> MapAsync(GuildResponse dto)
        => Map(dto,
            dto.AfkChannelId is { } afkId ? (GuildVoiceChannel)await app.ChannelsRepository.GetAsync(afkId) : null,
            dto.RulesChannelId is { } rulesId ? (GuildTextChannel)await app.ChannelsRepository.GetAsync(rulesId) : null,
            dto.SystemChannelId is { } systemId ? (GuildTextChannel)await app.ChannelsRepository.GetAsync(systemId) : null,
            await app.UsersRepository.GetAsync(dto.OwnerId)
        );

    [MapperIgnoreSource(nameof(GuildResponse.AfkChannelId))]
    [MapperIgnoreSource(nameof(GuildResponse.RulesChannelId))]
    [MapperIgnoreSource(nameof(GuildResponse.SystemChannelId))]
    [MapperIgnoreSource(nameof(GuildResponse.OwnerId))]
    private partial Guild Map(
        GuildResponse dto,
        GuildVoiceChannel? afkChannel,
        GuildTextChannel? systemChannel,
        GuildTextChannel? rulesChannel,
        IUser owner,
        FluxerApplication app
    );
    
    public Guild Map(
        GuildResponse dto,
        GuildVoiceChannel? afkChannel,
        GuildTextChannel? systemChannel,
        GuildTextChannel? rulesChannel,
        GlobalUser owner
    ) => Map(dto, afkChannel, systemChannel, rulesChannel, owner, app);

    public partial void UpdateEntity([MappingTarget] Guild data, Guild update);

    internal Guild MapCached(GuildResponse response)
        => Map(
            response,
            response.AfkChannelId is { } afkId ? app.ChannelsRepository.GetCachedOrDefault<GuildVoiceChannel>(afkId) : null,
            response.RulesChannelId is { } rulesId ? app.ChannelsRepository.GetCachedOrDefault<GuildTextChannel>(rulesId) : null,
            response.SystemChannelId is { } systemId
                ? app.ChannelsRepository.GetCachedOrDefault<GuildTextChannel>(systemId)
                : null,
            app.UsersRepository.GetCachedOrDefault(response.OwnerId) ?? new GlobalUser { Id = response.OwnerId }
        );
}