using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Users;

namespace Fluxify.Rest.Users;

public class PrivateChannelRequestBuilder(HttpClient client)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    
    private const string ChannelsUrl = "users/@me/channels";
    private const string PreloadUrl = "users/@me/channels/preload";
    private static readonly CompositeFormat PinUrl = CompositeFormat.Parse("users/@me/channels/{0}/pin");
    
    public async Task<ChannelResponse[]?> ListAsync(CancellationToken cancellationToken = default) 
        => await client.JsonRequestAsync<ChannelResponse[]>(
            HttpMethod.Get,
            ChannelsUrl,
            cancellationToken: cancellationToken
        );

    public async Task<ChannelResponse?> CreateAsync(
        CreatePrivateChannelRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<CreatePrivateChannelRequest, ChannelResponse>(
        HttpMethod.Post,
        ChannelsUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task<Dictionary<Snowflake, MessageResponse>?> PreloadMessagesAsync(
        PreloadMessagesRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<PreloadMessagesRequest, Dictionary<Snowflake, MessageResponse>>(
        HttpMethod.Post,
        PreloadUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task PinChannelAsync(
        Snowflake channelId,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Put,
        string.Format(FormatProvider, PinUrl, channelId),
        cancellationToken: cancellationToken
    );
    
    public async Task UnpinChannelAsync(
        Snowflake channelId,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, PinUrl, channelId),
        cancellationToken: cancellationToken
    );
}