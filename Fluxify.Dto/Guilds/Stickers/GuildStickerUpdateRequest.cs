namespace Fluxify.Dto.Guilds.Stickers;

record GuildStickerUpdateRequest(
    string? Description,
    string? Name,
    string[]? Tags
);