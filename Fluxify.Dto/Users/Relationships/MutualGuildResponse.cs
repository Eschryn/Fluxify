using Fluxify.Core.Types;

namespace Fluxify.Dto.Users.Relationships;

public record MutualGuildResponse(
    Snowflake Id,
    string? Nick
);