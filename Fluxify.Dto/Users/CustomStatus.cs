using Fluxify.Core;

namespace Fluxify.Dto.Users;

public record CustomStatus(
    Snowflake? EmojiId,
    string? EmojiName,
    DateTimeOffset? ExpiresAt,
    string? Text
);