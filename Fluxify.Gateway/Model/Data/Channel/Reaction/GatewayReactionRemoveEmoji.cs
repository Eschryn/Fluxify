using Fluxify.Core;
using Fluxify.Dto.Guilds.Emoji;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayReactionRemoveEmoji(Snowflake ChannelId, Snowflake MessageId, GuildEmojiResponse Emoji);