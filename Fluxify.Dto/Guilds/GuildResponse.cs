using System.Text.Json.Serialization;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Dto.Guilds;

public record GuildResponse(
    Snowflake? AfkChannelId,
    int AfkTimeout,
    [property: JsonPropertyName("banner")] string? BannerHash,
    int? BannerHeight,
    int? BannerWidth,
    DefaultMessageNotifications DefaultMessageNotifications,
    GuildOperations DisabledOperations,
    [property: JsonPropertyName("embed_splash")]
    string? EmbedSplashHash,
    int? EmbedSplashHeight,
    int? EmbedSplashWidth,
    GuildExplicitContentFilter ExplicitContentFilter,
    GuildFeatureSchema[] Features,
    [property: JsonPropertyName("icon")] string? IconHash,
    Snowflake? Id,
    DateTimeOffset? MessageHistoryCutoff,
    GuildMfaLevel MfaLevel,
    string Name,
    NsfwLevel NsfwLevel,
    Snowflake OwnerId,
    Permissions? Permissions,
    Snowflake? RulesChannelId,
    [property: JsonPropertyName("splash")] string? SplashHash,
    SplashCardAlignment SplashCardAlignment,
    int? SplashHeight,
    int? SplashWidth,
    SystemChannelFlags SystemChannelFlags,
    Snowflake? SystemChannelId,
    string? VanityUrlCode,
    GuildVerificationLevel VerificationLevel
);