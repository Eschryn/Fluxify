using Fluxify.Core;

namespace Fluxify.Dto.Guilds.AuditLog;

public record GuildAuditLogEntryResponse(
    AuditLogActionType ActionType,
    AuditLogChangeSchema[]? Changes,
    Snowflake Id,
    GuildAuditLogEntryResponseOptions? Options,
    string? Reason,
    Snowflake? TargetId,
    Snowflake? UserId);