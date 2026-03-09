using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.AuditLog;

public record GuildAuditLogListResponse(
    GuildAuditLogEntryResponse[] Entries,
    UserResponse[] User,
    AuditLogWebhookResponse[] Webhooks
);