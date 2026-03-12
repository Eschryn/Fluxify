using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.GroupDm;

public record ChannelUpdateGroupDmRequest(
    string? Icon,
    string? Name,
    Dictionary<string, string?> Nicks,
    Snowflake? OwnerId
) : ChannelUpdateRequest;