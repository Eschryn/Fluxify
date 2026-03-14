using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Voice;

namespace Fluxify.Rest.Channel;

public class ChannelRequestBuilder(HttpClient client, Snowflake id)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}");
    private static readonly CompositeFormat OverwriteUrl = CompositeFormat.Parse("channels/{0}/permissions/{1}");
    private static readonly CompositeFormat RecipientUrl = CompositeFormat.Parse("channels/{0}/recipients/{1}");
    private static readonly CompositeFormat RtcRegionUrl = CompositeFormat.Parse("channels/{0}/rtc-regions");
    private static readonly CompositeFormat TypingUrl = CompositeFormat.Parse("channels/{0}/typing");
    private static string Uri(CompositeFormat format, Snowflake id) => string.Format(FormatProvider, format, id);

    public MessagesRequestBuilder Messages { get; } = new(client, id);
    public CallRequestBuilder Call { get; } = new(client, id);
    
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

    public async Task DeleteAsync(bool? silent = null, CancellationToken cancellationToken = default) 
        => await client.RequestAsync(
            HttpMethod.Delete,
            Uri(GetUrl, id) + new QueryBuilder()
                .AddQuery("silent", silent?.ToString().ToLowerInvariant()),
            cancellationToken: cancellationToken
        );

    public async Task SetPermissionsOverwriteAsync(ChannelPermissionOverwrite overwrite, CancellationToken cancellationToken = default) 
        => await client.JsonRequestAsync(
            HttpMethod.Put,
            string.Format(FormatProvider, OverwriteUrl, id, overwrite.Id),
            overwrite,
            cancellationToken: cancellationToken
        );
    
    public async Task RemovePermissionsOverwriteAsync(Snowflake overwriteId, CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Delete,
            string.Format(FormatProvider, OverwriteUrl, id, overwriteId),
            cancellationToken: cancellationToken
        );

    /// <summary>
    /// Adds a user to a group DM
    /// </summary>
    /// <param name="userId">The user to be added to the group dm</param>
    /// <param name="cancellationToken"></param>
    public async Task AddRecipientAsync(Snowflake userId, CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Put,
            string.Format(FormatProvider, RecipientUrl, id, userId),
            cancellationToken: cancellationToken
        );
    
    public async Task RemoveRecipientAsync(Snowflake userId, bool? silent = null, CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Delete,
            string.Format(FormatProvider, RecipientUrl, id, userId) + new QueryBuilder()
                .AddQuery("silent", silent?.ToString().ToLowerInvariant()),
            cancellationToken: cancellationToken
        );

    public async Task<RtcRegion[]?> GetRtcRegionsAsync(CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<RtcRegion[]>(
            HttpMethod.Get,
            Uri(RtcRegionUrl, id),
            cancellationToken: cancellationToken
        );
    
    public async Task IndicateTypingAsync(CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Post,
            Uri(TypingUrl, id),
            cancellationToken: cancellationToken
        );
    
    // TODO: get, upload and update stream
}