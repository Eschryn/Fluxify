namespace Fluxify.Dto.Guilds.Stickers;

public record GuildStickerCreateBulkResponse(
    GuildStickerResponse[] Success,
    GuildStickerBulkFailedResponse[] Failed
);