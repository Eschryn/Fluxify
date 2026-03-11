using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text.Messages.Scheduled;

public record ScheduledMessageReferenceSchema(
    Snowflake? ChannelId,
    Snowflake? GuildId,
    Snowflake MessageId,
    MessageReferenceType? Type
);