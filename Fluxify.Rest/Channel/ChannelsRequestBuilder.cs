using Fluxify.Core.Types;

namespace Fluxify.Rest.Channel;

public class ChannelsRequestBuilder(HttpClient client)
{
    public ChannelRequestBuilder this[Snowflake id] => new(client, id);
}