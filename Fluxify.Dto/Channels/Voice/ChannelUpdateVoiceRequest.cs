using Fluxify.Core;

namespace Fluxify.Dto.Channels.Voice;

public record ChannelUpdateVoiceRequest(
    int? Bitrate,
    string? Name,
    Snowflake? OwnerId,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[] PermissionOverwrites,
    int? RateLimitPerUser,
    Snowflake? RtcRegion,
    int? UserLimit,
    string? Url,
    ChannelType Type = ChannelType.VoiceChannel
);