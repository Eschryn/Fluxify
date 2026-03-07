namespace Fluxify.Dto.Guilds.Emoji;

public record GuildEmojiBulkCreateResponse(GuildEmojiBulkCreateResponseFailedItem[] Failed, GuildEmojiResponse[] Success);