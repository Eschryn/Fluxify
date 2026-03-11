using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Voice;

public record ChannelCreateVoiceRequest(
    int? Bitrate,
    string Name,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[]? PermissionOverwrites,
    int? UserLimit
) : ChannelCreateRequest(
    Name,
    ParentId,
    PermissionOverwrites);