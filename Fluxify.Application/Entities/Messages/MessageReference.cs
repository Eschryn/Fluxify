using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Messages;

public class MessageReference
{
    public Snowflake MessageId { get; init; }
    public Snowflake? GuildId { get; init; }
    public Snowflake ChannelId { get; init; }
    public MessageReferenceType Type { get; init; }
}