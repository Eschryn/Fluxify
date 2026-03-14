using System.Globalization;
using System.Text;
using Fluxify.Core.Types;

namespace Fluxify.Rest.Channel.Messages;

public class MessageReactionsRequestBuilder(HttpClient client, Snowflake channelId, Snowflake messageId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}/messages/{1}/reactions");
    private static string Uri(CompositeFormat format, Snowflake channelId, Snowflake messageId) 
        => string.Format(FormatProvider, format, channelId, messageId);
    
    public MessageReactionRequestBuilder this[string emoji] => new(client, channelId, messageId, emoji);
    
    public async Task RemoveAllReactionsAsync(CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Delete,
            Uri(GetUrl, channelId, messageId),
            cancellationToken: cancellationToken
        );
}