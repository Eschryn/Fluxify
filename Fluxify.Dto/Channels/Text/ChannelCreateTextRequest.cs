using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text;

public record ChannelCreateTextRequest(
    string Name,
    bool? Nsfw,
    Snowflake? ParentId,
    ChannelPermissionOverwrite[]? PermissionOverwrites,
    string? Topic
) : ChannelCreateRequest(
    Name,
    ParentId,
    PermissionOverwrites);