using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text;

public record ChannelUpdateTextRequest(
    string? Name = null,
    bool? Nsfw = null,
    Snowflake? ParentId = null,
    ChannelPermissionOverwrite[]? PermissionOverwrites = null,
    int? RateLimitPerUser = null,
    string? Topic = null
) : ChannelUpdateRequest;