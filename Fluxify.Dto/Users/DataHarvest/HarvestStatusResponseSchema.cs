using Fluxify.Core;

namespace Fluxify.Dto.Users.DataHarvest;

public record HarvestStatusResponseSchema(
    DateTimeOffset? CompletedAt,
    DateTimeOffset CreatedAt,
    DateTimeOffset? DownloadUrlExpiresAt,
    string? ErrorMessage,
    DateTimeOffset? ExpiresAt,
    DateTimeOffset? FailedAt,
    string? FileSize,
    Snowflake HarvestId,
    double ProgressPercentage,
    string? ProgressStep,
    DateTimeOffset? StartedAt,
    HarvestStatusEnum Status
);