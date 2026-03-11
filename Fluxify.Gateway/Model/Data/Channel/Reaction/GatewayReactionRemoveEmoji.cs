using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Emoji;

namespace Fluxify.Gateway.Model.Data.Channel.Reaction;

public record GatewayReactionRemoveEmoji(Snowflake ChannelId, Snowflake MessageId, GuildEmojiResponse Emoji);