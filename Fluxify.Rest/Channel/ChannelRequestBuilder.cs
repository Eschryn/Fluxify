using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;

namespace Fluxify.Rest.Channel;

public class ChannelRequestBuilder(HttpClient client, Snowflake id)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}");
    private static string Uri(CompositeFormat format, Snowflake id) => string.Format(FormatProvider, format, id);

    public MessagesRequestBuilder Messages => new(client, id);
    public CallRequestBuilder Call => new(client, id);
    
    public async Task<ChannelResponse?> GetAsync(CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<ChannelResponse>(
            HttpMethod.Get,
            Uri(GetUrl, id),
            cancellationToken: cancellationToken);

    public async Task<ChannelResponse?> UpdateAsync(
        ChannelUpdateRequest request, 
        string? reason = null,
        CancellationToken cancellationToken = default
    ) =>
        await client.JsonRequestAsync<ChannelUpdateRequest, ChannelResponse>(
            HttpMethod.Patch, 
            Uri(GetUrl, id), request, 
            reason: reason, 
            cancellationToken: cancellationToken);

    public async Task<bool> DeleteAsync(bool silent)
    {
        var uriBuilder = new UriBuilder(Uri(GetUrl, id));
        if (silent)
        {
            uriBuilder.Query = "?silent";
        }

        var response = await client.DeleteAsync(uriBuilder.Uri);

        return response.IsSuccessStatusCode;
    }
}