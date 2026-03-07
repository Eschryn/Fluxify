using Fluxify.Core;

namespace Fluxify.Dto.Channels.Text.Messages;

public record MessageReferenceResponse(
    Snowflake ChannelId,
    Snowflake? GuildId,
    Snowflake MessageId,
    MessageReferenceType Type
);