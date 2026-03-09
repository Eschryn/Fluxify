using Fluxify.Core;

namespace Fluxify.Dto.Users.Relationships;

public record Relationship(
    Snowflake Id,
    string? Nickname,
    DateTimeOffset? Since,
    RelationshipTypes Type,
    UserResponse User
);