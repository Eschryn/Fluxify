using Fluxify.Dto.Common;
using Fluxify.Dto.Invites;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Packs;

public record PackInviteMetadataResponse(
    string Code,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt,
    UserPartialResponse? Inviter,
    int MaxUses,
    PackInviteMetadataResponsePack Pack,
    bool Temporary,
    int Uses
) : InviteMetadataResponseSchema(
    Code,
    CreatedAt,
    ExpiresAt,
    Inviter,
    MaxUses,
    Temporary,
    Uses
);