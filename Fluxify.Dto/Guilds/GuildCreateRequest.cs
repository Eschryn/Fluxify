namespace Fluxify.Dto.Guilds;

/// <summary>
/// 
/// </summary>
/// <param name="EmptyFeatures"></param>
/// <param name="Icon">Base64 encoded image data</param>
/// <param name="Name"></param>
public record GuildCreateRequest(
    bool? EmptyFeatures,
    string? Icon,
    string Name
);