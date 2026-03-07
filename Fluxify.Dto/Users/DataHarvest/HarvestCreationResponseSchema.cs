using Fluxify.Core;

namespace Fluxify.Dto.Users.DataHarvest;

public record HarvestCreationResponseSchema(
    DateTimeOffset CreatedAt,
    Snowflake HarvestId,
    HarvestStatusEnum Status
);