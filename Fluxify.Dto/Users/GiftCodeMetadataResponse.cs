namespace Fluxify.Dto.Users;

public record GiftCodeMetadataResponse(
    string Code,
    DateTimeOffset CreatedAt,
    User CreatedBy,
    long DurationMonths,
    DateTimeOffset? RedeemedAt,
    User? RedeemedBy
);