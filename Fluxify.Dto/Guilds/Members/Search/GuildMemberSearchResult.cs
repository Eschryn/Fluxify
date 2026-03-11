using Fluxify.Core.Types;

namespace Fluxify.Dto.Guilds.Members.Search;

public record GuildMemberSearchResult(
    string Discriminator,
    string? GlobalName,
    Snowflake GuildId,
    Snowflake Id,
    bool IsBot,
    int JoinedAt,
    string? Nickname,
    Snowflake[] RoleIds,
    GuildMemberSearchResultSupplemental Supplemental,
    Snowflake UserId,
    string Username
);