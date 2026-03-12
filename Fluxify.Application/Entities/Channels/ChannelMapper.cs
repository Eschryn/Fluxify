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
                        : null);

    public IChannel FromDto(ChannelResponse dto, GuildCategory? parent)
    {
        return dto.Type switch
        {
            ChannelType.TextChannel => TextChannelFromDto(dto, application, parent),
            ChannelType.VoiceChannel => VoiceChannelFromDto(dto, application, parent),
            ChannelType.GroupDm => GroupDmFromDto(dto, application),
            ChannelType.Category => CategoryFromDto(dto, application),
            ChannelType.LinkChannel => LinkChannelFromDto(dto, application, parent),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

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
    private partial GuildTextChannel TextChannelFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        GuildCategory? parent);

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
    private partial GuildVoiceChannel VoiceChannelFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        GuildCategory? parent);

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
        FluxerApplication fluxerApplication);

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
    private partial GuildLinkChannel LinkChannelFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication,
        GuildCategory? parent);

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
    private partial GuildCategory CategoryFromDto(
        ChannelResponse dto,
        FluxerApplication fluxerApplication);

    public partial IChannel UpdateEntity(IChannel data, IChannel update);
}