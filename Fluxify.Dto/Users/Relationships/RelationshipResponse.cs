using Fluxify.Core.Types;

namespace Fluxify.Dto.Users.Relationships;

public record RelationshipResponse(
    Snowflake Id,
    string? Nickname,
    DateTimeOffset? Since,
    RelationshipTypes Type,
    UserPartialResponse UserPartial
);