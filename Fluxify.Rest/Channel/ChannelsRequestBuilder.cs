using Fluxify.Core;

namespace Fluxify.Rest.Channel;

public class ChannelsRequestBuilder(HttpClient client)
{
    public ChannelRequestBuilder this[Snowflake id] => new(client, id);
}