using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Dto.Guilds.AuditLog;

public record AuditLogWebhookResponse(
    string? AvatarHash,
    Snowflake? ChannelId,
    Snowflake? GuildId,
    Snowflake Id,
    string Name,
    WebhookType type);