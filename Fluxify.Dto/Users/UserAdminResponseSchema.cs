using Fluxify.Core;
using Fluxify.Dto.Admin;

namespace Fluxify.Dto.Users;

public record UserAdminResponseSchema(
    int? AccentColor,
    string[] Acls,
    UserAuthenticatorTypes[] AuthenticatorTypes,
    string? Avatar,
    string? Banner,
    string? Bio,
    bool Bot,
    DateTimeOffset DateOfBirth,
    string? DeletionPublicReason,
    int? DeletionReasonCode,
    int Discriminator,
    string? Email,
    bool EmailBounced,
    bool EmailVerified,
    UserFlags Flags,
    string? GlobalName,
    bool HasTotp,
    Snowflake Id,
    DateTimeOffset? LastActiveAt,
    string? LastActiveIp,
    string? LastActiveIpReverse,
    string? LastActiveLocation,
    string? Locale,
    DateTimeOffset? PendingBulkMessageDeletionAt,
    DateTimeOffset? PendingDeletionAt,
    string? Phone,
    DateTimeOffset? PremiumSince,
    int? PremiumType,
    DateTimeOffset? PremiumUntil,
    string? Pronouns,
    SuspiciousActivityFlags SuspiciousActivityFlags,
    bool System,
    string? TempBannedUntil,
    string[] Traits,
    string Username
);