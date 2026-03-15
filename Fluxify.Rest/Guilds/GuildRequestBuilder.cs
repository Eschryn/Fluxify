using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.AuditLog;
using Fluxify.Dto.Guilds.Invite;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Settings;
using Fluxify.Dto.Users.Settings.Security;
using Fluxify.Rest.Channel;

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

    public async Task<GuildResponse?> UpdateAsync(
        GuildUpdateRequest guildUpdateRequest,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildUpdateRequest, GuildResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, GetGuildUrl, guildId),
        guildUpdateRequest,
        cancellationToken: cancellationToken
    );

    public async Task<GuildAuditLogListResponse?> ListAuditLogAsync(
        int? limit = null,
        Snowflake? before = null,
        Snowflake? after = null,
        Snowflake? user = null,
        AuditLogActionType? actionType = null,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildAuditLogListResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, AuditLogsUrl, guildId) + new QueryBuilder()
            .AddQuery("limit", limit?.ToString())
            .AddQuery("before", before?.ToString())
            .AddQuery("after", after?.ToString())
            .AddQuery("user_id", user?.ToString())
            .AddQuery("action_type", actionType?.ToString()),
        cancellationToken: cancellationToken
    );

    public async Task<ChannelResponse?> CreateChannelAsync(
        ChannelCreateRequest channelCreateRequest,
        string? reason = null,
        CancellationToken cancellationToken = default
    )
        => await client.JsonRequestAsync<ChannelCreateRequest, ChannelResponse>(
            HttpMethod.Post,
            string.Format(FormatProvider, ChannelsUrl, guildId),
            channelCreateRequest,
            reason: reason,
            cancellationToken: cancellationToken
        );

    public async Task<ChannelResponse[]?> ListChannelsAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<ChannelResponse[]>(
        HttpMethod.Get,
        string.Format(FormatProvider, ChannelsUrl, guildId),
        cancellationToken: cancellationToken
    );

    public async Task UpdateChannelPositionAsync(
        ChannelPositionUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Patch,
        string.Format(FormatProvider, ChannelsUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task<GuildBanResponse?> BanAsync(
        Snowflake userId,
        int? deleteMessageDays = null,
        TimeSpan? banDuration = null,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildBanResponse>(
        HttpMethod.Put,
        string.Format(FormatProvider, BanUrl, guildId, userId) + new QueryBuilder()
            .AddQuery("delete_message_days", deleteMessageDays?.ToString())
            .AddQuery("duration", ((int?)banDuration?.TotalSeconds)?.ToString())
            .AddQuery("reason", reason),
        reason: reason,
        cancellationToken: cancellationToken
    );

    public async Task UnbanAsync(
        Snowflake userId,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, BanUrl, guildId, userId),
        reason: reason,
        cancellationToken: cancellationToken
    );

    public async Task DeleteAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        string.Format(FormatProvider, DeleteUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task<GuildResponse?> ToggleDetachedBannerAsync(
        EnabledRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<EnabledRequest, GuildResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, DetachedBannerUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task<GuildResponse?> ToggleTextChannelFlexibleNames(
        EnabledRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<EnabledRequest, GuildResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, FlexibleTextChannelNamesUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<GuildResponse?> TransferOwnershipAsync(
        GuildTransferOwnershipRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildTransferOwnershipRequest, GuildResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, TransferOwnershipUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<GuildVanityUrlResponse?> GetVanityUrlAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildVanityUrlResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, VanityUrl, guildId),
        cancellationToken: cancellationToken
    );
    
    public async Task<GuildVanityUrlResponse?> UpdateVanityUrlAsync(
        GuildVanityUrlUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildVanityUrlUpdateRequest, GuildVanityUrlResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, VanityUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task LeaveGuildAsync(
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, GetGuildUrl, guildId),
        cancellationToken: cancellationToken
    );
}