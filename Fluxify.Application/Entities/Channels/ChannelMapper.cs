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
using Fluxify.Application.Entities.Users;
using Fluxify.Dto.Channels;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Channels;

[Mapper]
public partial class ChannelMapper(FluxerApplication application) : IUpdateEntity<IChannel>
{
    public async Task<IChannel> FromDtoAsync(ChannelResponse dto)
        => FromDto(dto,
            parent: dto.ParentId is { } parentId
                        ? (GuildCategory)await application.Channels.GetAsync(parentId)
                        : null,
            guild: dto.GuildId is { } guildId ? await application.Guilds.GetAsync(guildId) : null);

    public IChannel FromDto(ChannelResponse dto, GuildCategory? parent)
        => FromDto(dto, parent, dto.GuildId is {} gId ? application.Guilds.Cache.GetCachedOrDefault<Guild>(gId)! : null);
    
    public IChannel FromDto(ChannelResponse dto, GuildCategory? parent, Guild? guild)
    {
        return dto.Type switch
        {
            ChannelType.TextChannel => TextChannelFromDto(dto, application, parent, guild!),
            ChannelType.VoiceChannel => VoiceChannelFromDto(dto, application, parent, guild!),
            ChannelType.GroupDm => GroupDmFromDto(dto, application),
            ChannelType.Category => CategoryFromDto(dto, application, guild!),
            ChannelType.LinkChannel => LinkChannelFromDto(dto, application, parent, guild!),
            ChannelType.Dm => DmFromDto(dto, application),
            _ => throw new ArgumentOutOfRangeException()
        };
    }


    [MapperIgnoreSource(nameof(ChannelResponse.Bitrate))]
    [MapperIgnoreSource(nameof(ChannelResponse.RtcRegion))]
    [MapperIgnoreSource(nameof(ChannelResponse.UserLimit))]
    [MapperIgnoreSource(nameof(ChannelResponse.OwnerId))]
    [MapperIgnoreSource(nameof(ChannelResponse.Nicks))]
    [MapperIgnoreSource(nameof(ChannelResponse.IconHash))]
    [MapperIgnoreSource(nameof(ChannelResponse.Recipients))]
    [MapperIgnoreSource(nameof(ChannelResponse.Type))]
    [MapperIgnoreSource(nameof(ChannelResponse.Nsfw))]
    [MapperIgnoreSource(nameof(ChannelResponse.Topic))]
    [MapperIgnoreSource(nameof(ChannelResponse.RateLimitPerUser))]
    [MapperIgnoreSource(nameof(ChannelResponse.ParentId))]
    [MapperIgnoreSource(nameof(ChannelResponse.Url))]
    [MapperIgnoreSource(nameof(ChannelResponse.GuildId))]
    [MapperIgnoreSource(nameof(ChannelResponse.Overwrites))]
    [MapperIgnoreSource(nameof(ChannelResponse.Name))]
    [MapperIgnoreSource(nameof(ChannelResponse.Position))]
    public partial Dm DmFromDto(ChannelResponse dto, FluxerApplication fluxerApplication);

    [MapperIgnoreSource(nameof(ChannelResponse.Type))]
    [MapperIgnoreSource(nameof(ChannelResponse.Bitrate))]
    [MapperIgnoreSource(nameof(ChannelResponse.RtcRegion))]
    [MapperIgnoreSource(nameof(ChannelResponse.UserLimit))]
    [MapperIgnoreSource(nameof(ChannelResponse.OwnerId))]
    [MapperIgnoreSource(nameof(ChannelResponse.Nicks))]
    [MapperIgnoreSource(nameof(ChannelResponse.IconHash))]
    [MapperIgnoreSource(nameof(ChannelResponse.Recipients))]
    [MapperIgnoreSource(nameof(ChannelResponse.Url))]
    [MapperIgnoreSource(nameof(ChannelResponse.ParentId))]
    [MapperIgnoreSource(nameof(ChannelResponse.GuildId))]
    private partial GuildTextChannel TextChannelFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        GuildCategory? parent,
        Guild guild);

    [MapperIgnoreSource(nameof(ChannelResponse.OwnerId))]
    [MapperIgnoreSource(nameof(ChannelResponse.Nicks))]
    [MapperIgnoreSource(nameof(ChannelResponse.IconHash))]
    [MapperIgnoreSource(nameof(ChannelResponse.Recipients))]
    [MapperIgnoreSource(nameof(ChannelResponse.Url))]
    [MapperIgnoreSource(nameof(ChannelResponse.Type))]
    [MapperIgnoreSource(nameof(ChannelResponse.Nsfw))]
    [MapperIgnoreSource(nameof(ChannelResponse.Topic))]
    [MapperIgnoreSource(nameof(ChannelResponse.RateLimitPerUser))]
    [MapperIgnoreSource(nameof(ChannelResponse.LastMessageId))]
    [MapperIgnoreSource(nameof(ChannelResponse.LastPinTimestamp))]
    [MapperIgnoreSource(nameof(ChannelResponse.ParentId))]
    [MapperIgnoreSource(nameof(ChannelResponse.GuildId))]
    private partial GuildVoiceChannel VoiceChannelFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        GuildCategory? parent,
        Guild guild);

    [MapperIgnoreSource(nameof(ChannelResponse.Type))]
    [MapperIgnoreSource(nameof(ChannelResponse.Bitrate))]
    [MapperIgnoreSource(nameof(ChannelResponse.RtcRegion))]
    [MapperIgnoreSource(nameof(ChannelResponse.UserLimit))]
    [MapperIgnoreSource(nameof(ChannelResponse.Type))]
    [MapperIgnoreSource(nameof(ChannelResponse.Nsfw))]
    [MapperIgnoreSource(nameof(ChannelResponse.ParentId))]
    [MapperIgnoreSource(nameof(ChannelResponse.Position))]
    [MapperIgnoreSource(nameof(ChannelResponse.GuildId))]
    [MapperIgnoreSource(nameof(ChannelResponse.Overwrites))]
    [MapperIgnoreSource(nameof(ChannelResponse.Topic))]
    [MapperIgnoreSource(nameof(ChannelResponse.RateLimitPerUser))]
    [MapperIgnoreSource(nameof(ChannelResponse.Url))]
    [MapperIgnoreSource(nameof(ChannelResponse.Recipients))]
    private partial GroupDm GroupDmFromDto(
        ChannelResponse dto,
        IUser[]? recipients,
        FluxerApplication fluxerApplication);
    
    private GroupDm GroupDmFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication) => GroupDmFromDto(dto, dto.Recipients?.Select(u => (IUser?)application.Users.GetCachedOrDefault(u.Id) ?? application.Users.Insert(u)).ToArray(), fluxerApplication);

    [MapperIgnoreSource(nameof(ChannelResponse.Bitrate))]
    [MapperIgnoreSource(nameof(ChannelResponse.RtcRegion))]
    [MapperIgnoreSource(nameof(ChannelResponse.UserLimit))]
    [MapperIgnoreSource(nameof(ChannelResponse.OwnerId))]
    [MapperIgnoreSource(nameof(ChannelResponse.Nicks))]
    [MapperIgnoreSource(nameof(ChannelResponse.IconHash))]
    [MapperIgnoreSource(nameof(ChannelResponse.Recipients))]
    [MapperIgnoreSource(nameof(ChannelResponse.Type))]
    [MapperIgnoreSource(nameof(ChannelResponse.Nsfw))]
    [MapperIgnoreSource(nameof(ChannelResponse.Topic))]
    [MapperIgnoreSource(nameof(ChannelResponse.RateLimitPerUser))]
    [MapperIgnoreSource(nameof(ChannelResponse.LastMessageId))]
    [MapperIgnoreSource(nameof(ChannelResponse.LastPinTimestamp))]
    [MapperIgnoreSource(nameof(ChannelResponse.ParentId))]
    [MapperIgnoreSource(nameof(ChannelResponse.GuildId))]
    private partial GuildLinkChannel LinkChannelFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        GuildCategory? parent,
        Guild guild);

    [MapperIgnoreSource(nameof(ChannelResponse.Bitrate))]
    [MapperIgnoreSource(nameof(ChannelResponse.RtcRegion))]
    [MapperIgnoreSource(nameof(ChannelResponse.UserLimit))]
    [MapperIgnoreSource(nameof(ChannelResponse.OwnerId))]
    [MapperIgnoreSource(nameof(ChannelResponse.Nicks))]
    [MapperIgnoreSource(nameof(ChannelResponse.IconHash))]
    [MapperIgnoreSource(nameof(ChannelResponse.Recipients))]
    [MapperIgnoreSource(nameof(ChannelResponse.ParentId))]
    [MapperIgnoreSource(nameof(ChannelResponse.Url))]
    [MapperIgnoreSource(nameof(ChannelResponse.Type))]
    [MapperIgnoreSource(nameof(ChannelResponse.Nsfw))]
    [MapperIgnoreSource(nameof(ChannelResponse.Topic))]
    [MapperIgnoreSource(nameof(ChannelResponse.RateLimitPerUser))]
    [MapperIgnoreSource(nameof(ChannelResponse.LastMessageId))]
    [MapperIgnoreSource(nameof(ChannelResponse.LastPinTimestamp))]
    [MapperIgnoreSource(nameof(ChannelResponse.GuildId))]
    private partial GuildCategory CategoryFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        Guild guild);

    public partial void UpdateEntity([MappingTarget] IChannel data, IChannel update);
}