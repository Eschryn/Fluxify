using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using Fluxify.Core;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.LinkChannel;

namespace Fluxify.Rest.Channel;

public class ChannelRequestBuilder(HttpClient client, Snowflake id)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}");
    private static string Uri(CompositeFormat format, Snowflake id) => string.Format(FormatProvider, format, id);

    public async Task<ChannelResponse?> GetAsync(CancellationToken cancellationToken = default)
        => await client.GetFromJsonAsync<ChannelResponse>(Uri(GetUrl, id),
            RestClient.DefaultJsonOptions, cancellationToken);

    public async Task<ChannelResponse?> PatchAsync(ChannelUpdateRequest request)
    {
        var response = await client.PatchAsJsonAsync(Uri(GetUrl, id), request,
            RestClient.DefaultJsonOptions);
        return await response.EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<ChannelResponse>();
    }

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