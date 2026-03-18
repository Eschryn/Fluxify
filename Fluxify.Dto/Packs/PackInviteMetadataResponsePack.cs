using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Packs;

public record PackInviteMetadataResponsePack(
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    Snowflake Id,
    string Name,
    string? Description,
    UserPartialResponse Creator,
    Snowflake CreatorId,
    PackType Type
);