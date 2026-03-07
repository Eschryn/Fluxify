namespace Fluxify.Dto.Users.DataHarvest;

public record HarvestDownloadUrlResponse(
    string DownloadUrl,
    DateTimeOffset ExpiresAt
);