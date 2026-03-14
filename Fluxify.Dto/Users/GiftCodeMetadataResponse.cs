namespace Fluxify.Dto.Users;

public record GiftCodeMetadataResponse(
    string Code,
    DateTimeOffset CreatedAt,
    UserPartialResponse CreatedBy,
    long DurationMonths,
    DateTimeOffset? RedeemedAt,
    UserPartialResponse? RedeemedBy
);