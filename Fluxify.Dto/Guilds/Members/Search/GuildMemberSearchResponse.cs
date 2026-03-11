using Fluxify.Core.Types;

namespace Fluxify.Dto.Guilds.Members.Search;

public record GuildMemberSearchResponse(
    Snowflake GuildId,
    bool Indexing,
    GuildMemberSearchResult[] Members,
    long PageResultCount,
    long TotalResultCount);