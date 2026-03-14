using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Members;

public record GuildBanResponse(
    DateTimeOffset BannedAt,
    DateTimeOffset ExpiresAt,
    Snowflake ModeratorId,
    string? Reason,
    UserPartialResponse UserPartial);