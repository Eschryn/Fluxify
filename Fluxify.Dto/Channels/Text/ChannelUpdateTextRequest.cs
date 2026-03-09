using Fluxify.Core;
using Fluxify.Dto.Channels.LinkChannel;

namespace Fluxify.Dto.Channels.Text;

public record ChannelUpdateTextRequest(
    string? Name = null,
    bool? Nsfw = null,
    Snowflake? ParentId = null,
    ChannelPermissionOverwrite[]? PermissionOverwrites = null,
    int? RateLimitPerUser = null,
    string? Topic = null
) : ChannelUpdateRequest;