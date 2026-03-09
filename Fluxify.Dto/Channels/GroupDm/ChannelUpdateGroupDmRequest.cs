using Fluxify.Core;
using Fluxify.Dto.Channels.LinkChannel;

namespace Fluxify.Dto.Channels.GroupDm;

public record ChannelUpdateGroupDmRequest(
    string? Icon,
    string? Name,
    Dictionary<string, string?> Nicks,
    Snowflake? OwnerId
) : ChannelUpdateRequest;