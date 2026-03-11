using Fluxify.Dto.Guilds.Emoji;

namespace Fluxify.Gateway.Model.Data.Guild;

public record GatewayEmojiUpdate(GuildEmojiResponse[] Emojis);