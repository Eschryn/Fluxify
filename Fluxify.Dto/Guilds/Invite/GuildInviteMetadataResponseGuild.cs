using System.Text.Json.Serialization;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Dto.Guilds.Invite;

public record GuildInviteMetadataResponseGuild(
    [property: JsonPropertyName("banner")] string? BannerHash,
    int? BannerHeight,
    int? BannerWidth,
    string? EmbedSplash,
    int? EmbedSplashHeight,
    int? EmbedSplashWidth,
    GuildFeatureSchema[] Features,
    [property: JsonPropertyName("icon")] string? IconHash,
    Snowflake Id,
    string Name,
    [property: JsonPropertyName("splash")] string? SplashHash,
    SplashCardAlignment SplashCardAlignment,
    int SplashHeight,
    int SplashWidth);