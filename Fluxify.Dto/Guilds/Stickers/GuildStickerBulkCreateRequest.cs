namespace Fluxify.Dto.Guilds.Stickers;

public record GuildStickerBulkCreateRequest(
    GuildStickerCreateRequest[] Stickers);