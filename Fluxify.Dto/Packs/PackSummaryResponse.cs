using Fluxify.Core.Types;

namespace Fluxify.Dto.Packs;

public record PackSummaryResponse(
    DateTimeOffset CreatedAt,
    Snowflake CreatorId,
    string? Description,
    Snowflake Id,
    DateTimeOffset? InstalledAd,
    string Name,
    PackType Type,
    DateTimeOffset? UpdatedAt
);