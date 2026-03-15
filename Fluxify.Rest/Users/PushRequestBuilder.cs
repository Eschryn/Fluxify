// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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