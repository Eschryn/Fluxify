using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Common;
using Fluxify.Dto.Users.Push;

namespace Fluxify.Rest.Users;

public class PushRequestBuilder(HttpClient client)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    
    private const string SubscribeUrl = "users/@me/push/subscribe";
    private const string SubscriptionsUrl = "users/@me/push/subscriptions";
    private static readonly CompositeFormat SubscriptionUrl = CompositeFormat.Parse("users/@me/push/subscriptions/{0}");
    
    
    public async Task<PushSubscribeResponse?> SubscribeAsync(
        PushSubscribeRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<PushSubscribeRequest, PushSubscribeResponse>(
        HttpMethod.Post,
        SubscribeUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<PushSubscriptionListResponse?> GetSubscriptionsAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<PushSubscriptionListResponse>(
        HttpMethod.Get,
        SubscriptionsUrl,
        cancellationToken: cancellationToken
    );

    public async Task<bool?> UnsubscribeAsync(
        Snowflake subscriptionId,
        CancellationToken cancellationToken = default
    ) => (await client.JsonRequestAsync<SuccessResponse>(
        HttpMethod.Delete,
        string.Format(FormatProvider, SubscriptionUrl, subscriptionId),
        cancellationToken: cancellationToken
    ))?.Success;
}