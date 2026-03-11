using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.AuditLog;
using Fluxify.Dto.Guilds.Invite;

namespace Fluxify.Dto.Guilds.Members.Search;

public record GuildMemberSearchRequest(
    bool? IsBot,
    JoinSourceType[]? JoinSourceType,
    long? JoinedAtGte,
    long? JoinedAtLte,
    int? Limit,
    long? Offset,
    string? Query,
    Snowflake[]? RoleIds,
    GuildMemberSearchrequestSortBy? SortBy,
    SearchAuditLogsRequestSortOrder? SortOrder,
    string[]? SourceInviteCode,
    long? UserCreatedAtGte,
    long? UserCreatedAtLte);