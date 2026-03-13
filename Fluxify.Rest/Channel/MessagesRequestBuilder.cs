using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages;

namespace Fluxify.Rest.Channel;

public class MessagesRequestBuilder(HttpClient client, Snowflake id)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}/messages");
    private static string Uri(CompositeFormat format, Snowflake id) => string.Format(FormatProvider, format, id);
    
    public async Task<MessageResponse?> SendMessageAsync(MessageRequest request, CancellationToken cancellationToken = default)
        => await client.MultipartJsonRequestAsync<MessageRequest, MessageResponse>(
            HttpMethod.Post, 
            Uri(GetUrl, id),
            request,
            cancellationToken: cancellationToken
        );
}