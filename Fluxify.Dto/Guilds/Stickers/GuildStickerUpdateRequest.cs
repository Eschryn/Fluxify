namespace Fluxify.Dto.Guilds.Stickers;

public record GuildStickerUpdateRequest(
    string? Description,
    string? Name,
    string[]? Tags
);