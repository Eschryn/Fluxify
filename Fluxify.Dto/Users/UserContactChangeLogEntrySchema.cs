using Fluxify.Core;

namespace Fluxify.Dto.Users;

public record UserContactChangeLogEntrySchema(
    Snowflake? ActorUserId,
    string EventAt,
    Snowflake EventId,
    string Field,
    string? NewValue,
    string? OldValue,
    string? Reason
);