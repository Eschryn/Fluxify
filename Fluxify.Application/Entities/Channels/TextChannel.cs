using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Channels;

public abstract class TextChannel(
    FluxerApplication fluxerApplication
) : IChannel
{
    public async Task<Message?> SendMessageAsync(MessageDto message) 
        => await fluxerApplication.Messages.SendMessageAsync(Id, message);

    public required Snowflake Id { get; init; }
    public required string Name { get; init; }
    public Snowflake? LastMessageId { get; init; }
    public DateTimeOffset? LastPinTimestamp { get; init; }
}