using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.Channel;

public record ReadState(
    Snowflake Id,
    int MentionCount,
    Snowflake? LastMessageId,
    DateTimeOffset? LastPinTimestamp
);