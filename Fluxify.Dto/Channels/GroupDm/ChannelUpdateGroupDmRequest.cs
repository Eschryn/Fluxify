using Fluxify.Core;

namespace Fluxify.Dto.Channels.GroupDm;

public record ChannelUpdateGroupDmRequest(
    string? Icon,
    string? Name,
    Dictionary<string, string?> Nicks,
    Snowflake? OwnerId,
    ChannelType Type = ChannelType.GroupDm
);