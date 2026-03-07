using Fluxify.Core;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Members;

public record GuildBanResponse(
    DateTimeOffset BannedAt,
    DateTimeOffset ExpiresAt,
    Snowflake ModeratorId,
    string? Reason,
    User User);