namespace Fluxify.Dto.Guilds.Stickers;

/// <summary>
/// 
/// </summary>
/// <param name="Description"></param>
/// <param name="Image">Base64 encoded image</param>
public record GuildStickerCreateRequest(
    string? Description,
    string Image,
    string Name,
    string[]? Tags
);