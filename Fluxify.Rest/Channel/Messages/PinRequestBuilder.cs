using System.Globalization;
using System.Text;
using Fluxify.Core.Types;

namespace Fluxify.Rest.Channel;

public class PinRequestBuilder(HttpClient client, Snowflake channelId, Snowflake messageId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}/pins/{1}");
    private static string Uri(CompositeFormat format, Snowflake channelId, Snowflake messageId) 
        => string.Format(FormatProvider, format, channelId, messageId);

    public async Task PinAsync() => await client.RequestAsync(HttpMethod.Put, Uri(GetUrl, channelId, messageId));
    public async Task UnpinAsync() => await client.RequestAsync(HttpMethod.Delete, Uri(GetUrl, channelId, messageId));
}