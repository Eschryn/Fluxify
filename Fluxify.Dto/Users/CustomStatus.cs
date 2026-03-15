using Fluxify.Core.Types;

namespace Fluxify.Dto.Users;

public record CustomStatus(
    Snowflake? EmojiId = null,
    string? EmojiName = null,
    DateTimeOffset? ExpiresAt = null,
    string? Text = null
);