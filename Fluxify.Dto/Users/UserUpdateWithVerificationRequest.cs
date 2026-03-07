namespace Fluxify.Dto.Users;

public record UserUpdateWithVerificationRequest(
    int? AccentColor,
    string? Avatar,
    string? Banner,
    string? Bio,
    string? Discriminator,
    string? Email,
    string? EmailToken,
    string? GlobalName,
    bool? HasDismissedPremiumOnboarding,
    bool? HasUnreadGiftInventory,
    string? MfaCode,
    MfaMethod? MfaMethod,
    string? NewPassword,
    string? Password,
    bool? PremiumBadgeHidden,
    bool? PremiumBadgeMasked,
    bool? PremiumBadgeSequenceHidden,
    bool? PremiumBadgeTimestampHidden,
    bool? PremiumEnabledOverride,
    string? Pronouns,
    bool? UsedMobileClient,
    string? Username,
    string? WebauthnChallenge,
    string? WebauthnResponse
);