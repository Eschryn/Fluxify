using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Voice;

public record ChannelUpdateVoiceRequest(
    int? Bitrate,
    string? Name,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[] PermissionOverwrites,
    Snowflake? RtcRegion,
    int? UserLimit,
    ChannelType Type = ChannelType.VoiceChannel
);