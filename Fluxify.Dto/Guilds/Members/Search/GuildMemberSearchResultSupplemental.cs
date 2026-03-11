using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Invite;

namespace Fluxify.Dto.Guilds.Members.Search;

public record GuildMemberSearchResultSupplemental(
    Snowflake? InviterId,
    JoinSourceType? JoinSoureType,
    string? SourceInviteCode);