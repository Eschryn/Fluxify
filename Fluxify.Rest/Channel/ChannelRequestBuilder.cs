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
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Voice;
using Fluxify.Dto.Invites;
using Fluxify.Dto.Webhooks;
using Fluxify.Rest.Channel.Messages;

namespace Fluxify.Rest.Channel;

public class ChannelRequestBuilder(HttpClient client, Snowflake id)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}");
    private static readonly CompositeFormat OverwriteUrl = CompositeFormat.Parse("channels/{0}/permissions/{1}");
    private static readonly CompositeFormat RecipientUrl = CompositeFormat.Parse("channels/{0}/recipients/{1}");
    private static readonly CompositeFormat RtcRegionUrl = CompositeFormat.Parse("channels/{0}/rtc-regions");
    private static readonly CompositeFormat InvitesUrl = CompositeFormat.Parse("channels/{0}/invites");
    private static readonly CompositeFormat TypingUrl = CompositeFormat.Parse("channels/{0}/typing");
    private static readonly CompositeFormat WebhooksUrl = CompositeFormat.Parse("channels/{0}/webhooks");
    private static string Uri(CompositeFormat format, Snowflake id) => string.Format(FormatProvider, format, id);

    public MessagesRequestBuilder Messages { get; } = new(client, id);
    public CallRequestBuilder Call { get; } = new(client, id);

    public Task<ChannelResponse> GetAsync(CancellationToken cancellationToken = default)
        => client.JsonRequestAsync<ChannelResponse>(
            HttpMethod.Get,
            Uri(GetUrl, id),
            cancellationToken: cancellationToken
        );

    public Task<ChannelResponse> UpdateAsync(
        ChannelUpdateRequest request,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<ChannelUpdateRequest, ChannelResponse>(
        HttpMethod.Patch,
        Uri(GetUrl, id), request,
        reason: reason,
        cancellationToken: cancellationToken
    );

    public Task DeleteAsync(bool? silent = null, CancellationToken cancellationToken = default)
        => client.RequestAsync(
            HttpMethod.Delete,
            Uri(GetUrl, id) + new QueryBuilder()
                .AddQuery("silent", silent?.ToString().ToLowerInvariant()),
            cancellationToken: cancellationToken
        );

    public Task SetPermissionsOverwriteAsync(
        ChannelPermissionOverwrite overwrite,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Put,
        string.Format(FormatProvider, OverwriteUrl, id, overwrite.Id),
        overwrite,
        cancellationToken: cancellationToken
    );

    public Task RemovePermissionsOverwriteAsync(
        Snowflake overwriteId,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, OverwriteUrl, id, overwriteId),
        cancellationToken: cancellationToken
    );

    /// <summary>
    /// Adds a user to a group DM
    /// </summary>
    /// <param name="userId">The user to be added to the group dm</param>
    /// <param name="cancellationToken"></param>
    public Task AddRecipientAsync(
        Snowflake userId,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Put,
        string.Format(FormatProvider, RecipientUrl, id, userId),
        cancellationToken: cancellationToken
    );

    public Task RemoveRecipientAsync(
        Snowflake userId,
        bool? silent = null,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, RecipientUrl, id, userId) + new QueryBuilder()
            .AddQuery("silent", silent?.ToString().ToLowerInvariant()),
        cancellationToken: cancellationToken
    );

    public Task<RtcRegion[]> GetRtcRegionsAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<RtcRegion[]>(
        HttpMethod.Get,
        Uri(RtcRegionUrl, id),
        cancellationToken: cancellationToken
    );

    public Task IndicateTypingAsync(CancellationToken cancellationToken = default)
        => client.RequestAsync(
            HttpMethod.Post,
            Uri(TypingUrl, id),
            cancellationToken: cancellationToken
        );

    public Task<InviteMetadataResponseSchema[]> GetInvitesAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<InviteMetadataResponseSchema[]>(
        HttpMethod.Get,
        Uri(InvitesUrl, id),
        cancellationToken: cancellationToken
    );

    public Task<InviteMetadataResponseSchema> CreateInviteAsync(
        ChannelInviteCreateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<ChannelInviteCreateRequest, InviteMetadataResponseSchema>(
        HttpMethod.Post,
        Uri(InvitesUrl, id),
        request,
        cancellationToken: cancellationToken
    );

    public Task<WebhookResponse[]> GetWebhooksAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<WebhookResponse[]>(
        HttpMethod.Get,
        Uri(WebhooksUrl, id),
        cancellationToken: cancellationToken
    );

    public Task<WebhookResponse> CreateWebhookAsync(
        WebhookCreateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<WebhookCreateRequest, WebhookResponse>(
        HttpMethod.Post,
        Uri(WebhooksUrl, id),
        request,
        cancellationToken: cancellationToken
    );

    // TODO: get, upload and update stream
}