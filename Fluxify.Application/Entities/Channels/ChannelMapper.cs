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
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Channel;
using Fluxify.Application.State.Ref;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Category;
using Fluxify.Dto.Channels.LinkChannel;
using Fluxify.Dto.Channels.Text;
using Fluxify.Dto.Channels.Voice;
using Fluxify.Dto.Users;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Channels;

[Mapper]
public partial class ChannelMapper(FluxerApplication application)
    : IUpdateEntity<IChannel>, IUpdateEntity<IGuildChannel>
{
    public IChannel FromDto(ChannelResponse dto, CacheRef<Guild>? guildRef = null)
    {
        var guild = dto.GuildId is { } gId
            ? guildRef ?? (CacheRef<Guild>?)application.GuildsRepository.Cache.GetCachedOrDefault(gId)
            : null;

        return FromDto(
            dto,
            dto.ParentId is { } pId
                ? application.ChannelsRepository.Cache.GetCachedOrDefault(pId)
                : null,
            guild
        );
    }

    private CacheRef<GlobalUser> MapUser(UserPartialResponse response)
        => application.UsersRepository.Insert(response);

    [MapperIgnoreSource(nameof(VoiceChannelProperties.RtcRegion))]
    private partial ChannelCreateVoiceRequest ToCreateRequest(VoiceChannelProperties request);

    [MapDerivedType<VoiceChannelProperties, ChannelCreateVoiceRequest>]
    [MapDerivedType<TextChannelProperties, ChannelCreateTextRequest>]
    [MapDerivedType<LinkChannelProperties, ChannelCreateLinkRequest>]
    [MapDerivedType<CategoryProperties, ChannelCreateCategoryRequest>]
    public partial ChannelCreateRequest ToCreateRequest(ChannelProperties request);

    [MapDerivedType<VoiceChannelProperties, ChannelUpdateVoiceRequest>]
    [MapDerivedType<TextChannelProperties, ChannelUpdateTextRequest>]
    [MapDerivedType<LinkChannelProperties, ChannelUpdateLinkRequest>]
    [MapDerivedType<CategoryProperties, ChannelUpdateCategoryRequest>]
    public partial ChannelUpdateRequest ToUpdateRequest(ChannelProperties request);

    public PermissionOverwrite ToPermissionOverwrite(ChannelPermissionOverwrite dto)
        => dto.Type switch
        {
            PermissionOverwriteType.Member => new PermissionOverwrite.Member(dto.Id)
            {
                Allow = dto.Allow,
                Deny = dto.Deny
            },
            PermissionOverwriteType.Role => new PermissionOverwrite.Role(dto.Id)
            {
                Allow = dto.Allow,
                Deny = dto.Deny
            },
            _ => throw new ArgumentOutOfRangeException()
        };

    public partial ChannelPermissionOverwrite FromPermissionOverwrite(PermissionOverwrite overwrite);

    [MapperIgnoreSource(nameof(GuildChannel<,>.Id))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.Guild))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.GuildRef))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.Position))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.RequestBuilder))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.OverwritesDictionary))]
    [MapperIgnoreSource(nameof(GuildNestedChannel<,>.Position))]
    [MapperIgnoreSource(nameof(GuildNestedChannel<,>.ParentRef))]
    [MapperIgnoreSource(nameof(GuildTextChannel.MessageRepository))]
    [MapperIgnoreSource(nameof(GuildTextChannel.LastMessageId))]
    [MapperIgnoreSource(nameof(GuildTextChannel.LastPinTimestamp))]
    [MapProperty(nameof(GuildChannel<,>.Overwrites), nameof(TextChannelProperties.PermissionOverwrites))]
    private partial TextChannelProperties ToChannelProperties(GuildTextChannel request);

    [MapDerivedType<GuildVoiceChannel, VoiceChannelProperties>]
    [MapDerivedType<GuildTextChannel, TextChannelProperties>]
    [MapDerivedType<GuildLinkChannel, LinkChannelProperties>]
    [MapDerivedType<GuildCategory, CategoryProperties>]
    [MapperIgnoreSource(nameof(GuildChannel<,>.Id))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.Guild))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.GuildRef))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.Position))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.RequestBuilder))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.OverwritesDictionary))]
    [MapperIgnoreSource(nameof(GuildNestedChannel<,>.Position))]
    [MapperIgnoreSource(nameof(GuildNestedChannel<,>.ParentRef))]
    [MapProperty(nameof(GuildChannel<,>.Overwrites), nameof(TextChannelProperties.PermissionOverwrites))]
    private partial GuildChannelProperties ToProperties(IGuildChannel request);

    [MapperIgnoreSource(nameof(GroupDm.Id))]
    [MapperIgnoreSource(nameof(GroupDm.IconHash))]
    [MapperIgnoreSource(nameof(ITextChannel.LastMessageId))]
    [MapperIgnoreSource(nameof(ITextChannel.LastPinTimestamp))]
    [MapperIgnoreSource(nameof(PrivateTextChannel.MessageRepository))]
    [MapperIgnoreSource(nameof(PrivateTextChannel.RecipientsRef))]
    [MapValue(nameof(GroupDmProperties.Icon), null)]
    private partial GroupDmProperties ToProperties(GroupDm request);

    [MapDerivedType<GuildVoiceChannel, VoiceChannelProperties>]
    [MapDerivedType<GuildTextChannel, TextChannelProperties>]
    [MapDerivedType<GuildLinkChannel, LinkChannelProperties>]
    [MapDerivedType<GuildCategory, CategoryProperties>]
    [MapDerivedType<GroupDm, GroupDmProperties>]
    public partial ChannelProperties ToProperties(IChannel request);

    public IChannel FromDto(ChannelResponse dto, CacheRef<IChannel>? parent, CacheRef<Guild>? guild)
    {
        guild ??= dto.GuildId is { } guildId
            ? application.GuildsRepository.Cache.GetCachedOrDefault(guildId)
            : null;

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
    [MapProperty(nameof(ChannelResponse.Recipients), nameof(Dm.RecipientsRef))]
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
    [MapperIgnoreTarget(nameof(GuildChannel<,>.OverwritesDictionary))]
    private partial GuildTextChannel TextChannelFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        CacheRef<IChannel>? parentRef,
        CacheRef<Guild> guildRef);

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
    [MapperIgnoreTarget(nameof(GuildChannel<,>.OverwritesDictionary))]
    private partial GuildVoiceChannel VoiceChannelFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        CacheRef<IChannel>? parentRef,
        CacheRef<Guild> guildRef);

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
    private partial GroupDm GroupDmFromDto(
        ChannelResponse dto,
        CacheRef<GlobalUser>[]? recipientsRef,
        FluxerApplication fluxerApplication);

    private GroupDm GroupDmFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication) => GroupDmFromDto(dto,
        dto.Recipients?.Select(u => application.UsersRepository.Insert(u))
            .ToArray(), fluxerApplication);

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
    [MapperIgnoreTarget(nameof(GuildChannel<,>.OverwritesDictionary))]
    private partial GuildLinkChannel LinkChannelFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        CacheRef<IChannel>? parentRef,
        CacheRef<Guild> guildRef);

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
    [MapperIgnoreTarget(nameof(GuildChannel<,>.OverwritesDictionary))]
    private partial GuildCategory CategoryFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        CacheRef<Guild> guildRef);

    [MapDerivedType<Dm, Dm>]
    [MapDerivedType<GroupDm, GroupDm>]
    [MapperIgnoreSource(nameof(IChannel.Id))]
    public partial void UpdateEntity([MappingTarget] PrivateTextChannel data, PrivateTextChannel update);

    public void UpdateEntity([MappingTarget] IChannel data, IChannel update)
    {
        switch (data)
        {
            case IGuildChannel guildChannel:
                UpdateEntity(guildChannel, (IGuildChannel)update);
                return;
            case PrivateTextChannel privateTextChannel:
                UpdateEntity(privateTextChannel, (PrivateTextChannel)update);
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(data));
        }
    }

    [MapDerivedType<GuildVoiceChannel, GuildVoiceChannel>]
    [MapDerivedType<GuildTextChannel, GuildTextChannel>]
    [MapDerivedType<GuildLinkChannel, GuildLinkChannel>]
    [MapDerivedType<GuildCategory, GuildCategory>]
    [MapperIgnoreSource(nameof(IChannel.Id))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.Guild))]
    [MapperIgnoreSource(nameof(GuildChannel<,>.GuildRef))]
    public partial void UpdateEntity([MappingTarget] IGuildChannel data, IGuildChannel update);

    public partial PartialChannel FromPartialResponse(ChannelPartialResponse response);
}