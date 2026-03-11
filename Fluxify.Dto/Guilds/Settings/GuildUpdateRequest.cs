using Fluxify.Core.Types;

namespace Fluxify.Dto.Guilds.Settings;

public record GuildUpdateRequest(
    Snowflake? AfkChannelId,
    int? AfkTimeout,
    string? Banner,
    DefaultMessageNotifications? DefaultMessageNotifications,
    string? EmbedSplash,
    GuildExplicitContentFilter? ExplicitContentFilter,
    GuildFeatureSchema[]? Features,
    string? Icon,
    DateTimeOffset? MessageHistoryCutoff,
    string? MfaCode,
    GuildMfaLevel? MfaLevel,
    MfaMethod? MfaMethod,
    string? Name,
    NsfwLevel? NsfwLevel,
    string? Password,
    string? Splash,
    SplashCardAlignment? SplashCardAlignment,
    SystemChannelFlags? SystemChannelFlags,
    Snowflake? SystemChannelId,
    GuildVerificationLevel? VerificationLevel,
    string? WebauthnChallenge,
    string? WebauthnResponse
);