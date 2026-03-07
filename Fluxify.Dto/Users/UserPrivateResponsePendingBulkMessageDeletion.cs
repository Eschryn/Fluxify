namespace Fluxify.Dto.Users;

public record UserPrivateResponsePendingBulkMessageDeletion(
    int ChannelCount,
    int MessageCount,
    DateTimeOffset ScheduledAt
);