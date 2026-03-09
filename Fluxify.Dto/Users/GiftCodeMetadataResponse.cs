namespace Fluxify.Dto.Users;

public record GiftCodeMetadataResponse(
    string Code,
    DateTimeOffset CreatedAt,
    UserResponse CreatedBy,
    long DurationMonths,
    DateTimeOffset? RedeemedAt,
    UserResponse? RedeemedBy
);