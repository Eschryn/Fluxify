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

using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.State.Ref;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.Settings;
using Fluxify.Dto.Users;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Guilds;

[Mapper]
public partial class GuildMapper(FluxerApplication app) : IUpdateEntity<Guild>
{
    public async Task<Guild> MapAsync(GuildResponse dto)
        => Map(dto,
            dto.AfkChannelId is { } afkId ? await app.ChannelsRepository.GetAsync(afkId) : null,
            dto.RulesChannelId is { } rulesId ? await app.ChannelsRepository.GetAsync(rulesId) : null,
            dto.SystemChannelId is { } systemId ? await app.ChannelsRepository.GetAsync(systemId) : null,
            await app.UsersRepository.GetAsync(dto.OwnerId)
        );

    private static GuildOperations ToOperations(long operations) => (GuildOperations)operations;

    [MapperIgnoreSource(nameof(GuildResponse.AfkChannelId))]
    [MapperIgnoreSource(nameof(GuildResponse.RulesChannelId))]
    [MapperIgnoreSource(nameof(GuildResponse.SystemChannelId))]
    [MapperIgnoreSource(nameof(GuildResponse.OwnerId))]
    [MapperIgnoreTarget(nameof(Guild.GuildStickers))]
    [MapperIgnoreTarget(nameof(Guild.GuildEmojis))]
    private partial Guild Map(
        GuildResponse dto,
        CacheRef<IChannel>? afkChannelRef,
        CacheRef<IChannel>? systemChannelRef,
        CacheRef<IChannel>? rulesChannelRef,
        CacheRef<GlobalUser> ownerRef,
        FluxerApplication app
    );
    
    public Guild Map(
        GuildResponse dto,
        CacheRef<IChannel>? afkChannel,
        CacheRef<IChannel>? systemChannel,
        CacheRef<IChannel>? rulesChannel,
        CacheRef<GlobalUser> owner
    ) => Map(dto, afkChannel, systemChannel, rulesChannel, owner, app);

    [MapperIgnoreSource(nameof(Guild.AfkChannel))]
    [MapperIgnoreSource(nameof(Guild.RulesChannel))]
    [MapperIgnoreSource(nameof(Guild.SystemChannel))]
    public partial void UpdateEntity([MappingTarget] Guild data, Guild update);

    internal Guild MapCached(GuildResponse response)
        => Map(
            response,
            response.AfkChannelId is { } afkId ? app.ChannelsRepository.GetCachedOrDefault(afkId) : null,
            response.RulesChannelId is { } rulesId ? app.ChannelsRepository.GetCachedOrDefault(rulesId) : null,
            response.SystemChannelId is { } systemId
                ? app.ChannelsRepository.GetCachedOrDefault(systemId)
                : null,
            app.UsersRepository.GetCachedOrDefault(response.OwnerId) is { Value: not null } cacheRef 
                ? cacheRef 
                : app.UsersRepository.Insert(CreateUserFromOwnerId(response))
        );

    private static UserPartialResponse CreateUserFromOwnerId(GuildResponse response)
        => new(
            Id: response.OwnerId,
            Discriminator: "0000",
            Username: "Unknown",
            Avatar: null,
            AvatarColor: null,
            Bot: null,
            Flags: 0,
            GlobalName: null,
            System: null
        );
}
