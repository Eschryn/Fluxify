using Fluxify.Core;

namespace Fluxify.Dto.Users.Relationships;

public record MutualGuildResponse(
    Snowflake Id,
    string? Nick
);