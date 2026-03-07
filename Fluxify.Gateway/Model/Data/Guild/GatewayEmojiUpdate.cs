using Fluxify.Dto.Guilds.Emoji;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayEmojiUpdate(GuildEmojiResponse[] Emojis);