using System.Text.Json.Serialization;

namespace Fluxify.Dto.Users;

public record UserProfile(
    int? AccentColor,
    [property: JsonPropertyName("banner")] string? BannerHash,
    int? BannerColor,
    string? Bio,
    string? Pronouns
);