using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;
using Fluxify.Core.Types;
using Fluxify.Rest;

namespace Fluxify.Application.Services;

public class MessageService(
    RestClient client,
    MessageMapper mapper)
{
    public async Task<Message?> SendMessageAsync(Snowflake channelId, MessageDto messageDto, CancellationToken cancellationToken = default)
    {
        var result = await client.Channels[channelId].Messages
            .SendMessageAsync(mapper.Map(messageDto), cancellationToken);
        
        return result != null ? await mapper.MapAsync(result) : null;
    }
}