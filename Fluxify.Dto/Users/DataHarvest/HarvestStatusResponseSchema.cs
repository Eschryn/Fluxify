using Fluxify.Core.Types;

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
    double ProgressPercent,
    string? ProgressStep,
    DateTimeOffset? StartedAt,
    HarvestStatusEnum Status
);