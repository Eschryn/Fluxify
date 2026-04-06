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
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.AuditLog;
using Fluxify.Dto.Guilds.Invite;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Settings;
using Fluxify.Dto.Invites;
using Fluxify.Dto.Users.Settings.Security;
using Fluxify.Dto.Webhooks;

namespace Fluxify.Rest.Guilds;

public class GuildRequestBuilder(HttpClient client, Snowflake guildId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetGuildUrl = CompositeFormat.Parse("guilds/{0}");
    private static readonly CompositeFormat DeleteUrl = CompositeFormat.Parse("guilds/{0}/delete");
    private static readonly CompositeFormat TransferOwnershipUrl = CompositeFormat.Parse("guilds/{0}/transfer-ownership");
    private static readonly CompositeFormat VanityUrl = CompositeFormat.Parse("guilds/{0}/vanity-url");
    private static readonly CompositeFormat DetachedBannerUrl = CompositeFormat.Parse("guilds/{0}/detached-banner");
    private static readonly CompositeFormat AuditLogsUrl = CompositeFormat.Parse("guilds/{0}/audit-logs");
    private static readonly CompositeFormat ChannelsUrl = CompositeFormat.Parse("guilds/{0}/channels");
    private static readonly CompositeFormat BanUrl = CompositeFormat.Parse("guilds/{0}/bans/{1}");
    private static readonly CompositeFormat FlexibleTextChannelNamesUrl = CompositeFormat.Parse("guilds/{0}/text-channel-flexible-names");
    private static readonly CompositeFormat InvitesUrl = CompositeFormat.Parse("guilds/{0}/invites");
    private static readonly CompositeFormat WebhooksUrl = CompositeFormat.Parse("guilds/{0}/webhooks");

    public EmojisRequestBuilder Emojis { get; } = new(client, guildId);
    public MembersRequestBuilder Members { get; } = new(client, guildId);
    public RolesRequestBuilder Roles { get; } = new(client, guildId);
    public StickersRequestBuilder Stickers { get; } = new(client, guildId);

    public Task<GuildResponse?> GetAsync(CancellationToken cancellationToken = default)
        => client.JsonRequestAsync<GuildResponse>(
            HttpMethod.Get,
            string.Format(FormatProvider, GetGuildUrl, guildId),
            cancellationToken: cancellationToken
        );

    public Task<GuildResponse?> UpdateAsync(
        GuildUpdateRequest guildUpdateRequest,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildUpdateRequest, GuildResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, GetGuildUrl, guildId),
        guildUpdateRequest,
        cancellationToken: cancellationToken
    );

    public Task<GuildAuditLogListResponse?> ListAuditLogAsync(
        int? limit = null,
        Snowflake? before = null,
        Snowflake? after = null,
        Snowflake? user = null,
        AuditLogActionType? actionType = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildAuditLogListResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, AuditLogsUrl, guildId) + new QueryBuilder()
            .AddQuery("limit", limit?.ToString())
            .AddQuery("before", before?.ToString())
            .AddQuery("after", after?.ToString())
            .AddQuery("user_id", user?.ToString())
            .AddQuery("action_type", actionType?.ToString()),
        cancellationToken: cancellationToken
    );

    public Task<ChannelResponse?> CreateChannelAsync(
        ChannelCreateRequest channelCreateRequest,
        string? reason = null,
        CancellationToken cancellationToken = default
    )
        => client.JsonRequestAsync<ChannelCreateRequest, ChannelResponse>(
            HttpMethod.Post,
            string.Format(FormatProvider, ChannelsUrl, guildId),
            channelCreateRequest,
            reason: reason,
            cancellationToken: cancellationToken
        );

    public Task<ChannelResponse[]?> ListChannelsAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<ChannelResponse[]>(
        HttpMethod.Get,
        string.Format(FormatProvider, ChannelsUrl, guildId),
        cancellationToken: cancellationToken
    );

    public Task UpdateChannelPositionAsync(
        ChannelPositionUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Patch,
        string.Format(FormatProvider, ChannelsUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public Task<GuildBanResponse?> BanAsync(
        Snowflake userId,
        int? deleteMessageDays = null,
        TimeSpan? banDuration = null,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildBanResponse>(
        HttpMethod.Put,
        string.Format(FormatProvider, BanUrl, guildId, userId) + new QueryBuilder()
            .AddQuery("delete_message_days", deleteMessageDays?.ToString())
            .AddQuery("duration", ((int?)banDuration?.TotalSeconds)?.ToString())
            .AddQuery("reason", reason),
        reason: reason,
        cancellationToken: cancellationToken
    );

    public Task UnbanAsync(
        Snowflake userId,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, BanUrl, guildId, userId),
        reason: reason,
        cancellationToken: cancellationToken
    );

    public Task DeleteAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        string.Format(FormatProvider, DeleteUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public Task<GuildResponse?> ToggleDetachedBannerAsync(
        EnabledRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EnabledRequest, GuildResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, DetachedBannerUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public Task<GuildResponse?> ToggleTextChannelFlexibleNames(
        EnabledRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EnabledRequest, GuildResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, FlexibleTextChannelNamesUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public Task<GuildResponse?> TransferOwnershipAsync(
        GuildTransferOwnershipRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildTransferOwnershipRequest, GuildResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, TransferOwnershipUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public Task<GuildVanityUrlResponse?> GetVanityUrlAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildVanityUrlResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, VanityUrl, guildId),
        cancellationToken: cancellationToken
    );
    
    public Task<GuildVanityUrlResponse?> UpdateVanityUrlAsync(
        GuildVanityUrlUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildVanityUrlUpdateRequest, GuildVanityUrlResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, VanityUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public Task LeaveGuildAsync(
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, GetGuildUrl, guildId),
        cancellationToken: cancellationToken
    );
    
    public Task<InviteMetadataResponseSchema[]?> ListInvitesAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<InviteMetadataResponseSchema[]>(
        HttpMethod.Get,
        string.Format(FormatProvider, InvitesUrl, guildId),
        cancellationToken: cancellationToken
    );
    
    public Task<WebhookResponse[]?> GetWebhooksAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<WebhookResponse[]>(
        HttpMethod.Get,
        string.Format(FormatProvider, WebhooksUrl, guildId),
        cancellationToken: cancellationToken
    );
}