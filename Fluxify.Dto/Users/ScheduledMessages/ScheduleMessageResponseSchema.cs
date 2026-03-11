using Fluxify.Core.Types;

namespace Fluxify.Dto.Users.ScheduledMessages;

public record ScheduleMessageResponseSchema(
    Snowflake ChannelId,
    DateTimeOffset CreatedAt,
    Snowflake Id,
    DateTimeOffset? InvalidatedAt,
    ScheduledMessageResponseSchemaPayload Payload,
    DateTimeOffset ScheduledAt,
    DateTimeOffset ScheduledAtLocal,
    ScheduledMessageStatus Status,
    string? StatusReason,
    string Timezone
);