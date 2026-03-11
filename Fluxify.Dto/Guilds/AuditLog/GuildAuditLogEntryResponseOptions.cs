using Fluxify.Core.Types;

namespace Fluxify.Dto.Guilds.AuditLog;

public record GuildAuditLogEntryResponseOptions(
    Snowflake? ChannelId,
    int? Count,
    string? DeleteMemberDays,
    Snowflake? Id,
    int? IntegrationType,
    Snowflake? InviterId,
    double? MaxAge,
    int? MaxUses,
    int? MembersRemoved,
    Snowflake? MessageId,
    string? RoleName,
    bool? Temporary,
    int? Type,
    int? Uses
);