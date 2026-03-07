using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record ReadState(
    Snowflake Id,
    int MentionCount,
    Snowflake? LastMessageId,
    DateTimeOffset? LastPinTimestamp
);