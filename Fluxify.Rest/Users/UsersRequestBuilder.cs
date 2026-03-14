using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.GuildSettings;
using Fluxify.Dto.Users.Settings;
using Fluxify.Dto.Users.Settings.Security;

namespace Fluxify.Rest.Users;

public class UsersRequestBuilder(HttpClient client)
{
    private const string CheckTagUrl = "users/check-tag";

    public MeRequestBuilder Me { get; } = new(client);
    public UserRequestBuilder this[Snowflake userId] => new(client, userId);

    public async Task<UserTagCheckResponse?> CheckTagAsync(
        UserTagCheckRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<UserTagCheckRequest, UserTagCheckResponse>(
        HttpMethod.Get,
        CheckTagUrl,
        request,
        cancellationToken: cancellationToken
    );
}

public class MeRequestBuilder(HttpClient client)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private const string MeUrl = "users/@me";
    private const string PreloadUrl = "users/@me/preload-messages";
    private const string AuthorizedIpsUrl = "users/@me/authorized-ips";
    private const string DeleteUrl = "users/@me/delete";
    private const string DisableUrl = "users/@me/disable";
    private const string GiftsUrl = "users/@me/gifts";
    private const string DmSettingsUrl = "users/@me/guilds/@me/settings";
    private const string MentionsUrl = "users/@me/mentions";
    private const string SettingsUrl = "users/@me/settings";
    private const string ResetPremiumUrl = "users/@me/premium/reset";
    private static readonly CompositeFormat GuildSettingsUrl = CompositeFormat.Parse("users/@me/guilds/{0}/settings");
    private static readonly CompositeFormat MentionsIdUrl = CompositeFormat.Parse("users/@me/mentions/{0}");

    public PrivateChannelRequestBuilder PrivateChannels { get; } = new(client);
    public EmailChangeRequestBuilder EmailChange { get; } = new(client);
    public HarvestRequestBuilder Harvest { get; } = new(client);
    public UserMessagesRequestBuilder Messages { get; } = new(client);
    public MfaRequestBuilder Mfa { get; } = new(client);
    public NotesRequestBuilder Notes { get; } = new(client);
    public PasswordChangeRequestBuilder PasswordChange { get; } = new(client);
    public PhoneRequestBuilder Phone { get; } = new(client);
    public PushRequestBuilder Push { get; } = new(client);
    public RelationshipsRequestBuilder Relationships { get; } = new(client);
    public SavedMessagesRequestBuilder SavedMessages { get; } = new(client);
    public ScheduledMessagesRequestBuilder ScheduledMessages { get; } = new(client);
    public SudoRequestBuilder Sudo { get; } = new(client);

    public async Task<UserPrivate?> GetMeAsync(CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<UserPrivate>(
            HttpMethod.Get,
            MeUrl,
            cancellationToken: cancellationToken
        );

    public async Task<UserPrivate?> UpdateMeAsync(
        UserUpdateWithVerificationRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<UserUpdateWithVerificationRequest, UserPrivate>(
        HttpMethod.Patch,
        MeUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task ForgetAuthorizedIpAsync(SudoVerificationSchema request,
        CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<SudoVerificationSchema>(
            HttpMethod.Delete,
            AuthorizedIpsUrl,
            request,
            cancellationToken: cancellationToken
        );

    public async Task DeleteAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        DeleteUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task DisableAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        DisableUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task<GiftCodeMetadataResponse?> ListGiftsAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GiftCodeMetadataResponse>(
        HttpMethod.Get,
        GiftsUrl,
        cancellationToken: cancellationToken
    );

    public async Task<UserGuildSettingsResponse?> DmNotificationSettings(
        UserGuildSettingsRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<UserGuildSettingsRequest, UserGuildSettingsResponse>(
        HttpMethod.Patch,
        DmSettingsUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task<UserGuildSettingsResponse?> GuildNotificationSettings(
        Snowflake guildId,
        UserGuildSettingsRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<UserGuildSettingsRequest, UserGuildSettingsResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, GuildSettingsUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task<MessageResponse[]?> ListMentionsAsync(
        int? limit = null,
        bool? roles = null,
        bool? everyone = null,
        bool? guilds = null,
        Snowflake? before = null,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<MessageResponse[]>(
        HttpMethod.Get,
        MentionsUrl,
        cancellationToken: cancellationToken
    );

    public async Task DeleteMentionAsync(
        Snowflake messageId,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, MentionsIdUrl, messageId),
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <remarks>This endpoint is staff only!</remarks>
    public async Task ResetPremiumStatusAsync(CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Post,
            ResetPremiumUrl,
            cancellationToken: cancellationToken
        );

    public async Task<UserSettings?> GetUserSettingsAsync(CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<UserSettings>(
            HttpMethod.Get,
            SettingsUrl,
            cancellationToken: cancellationToken
        );

    public async Task<UserSettings?> UpdateUserSettingsAsync(
        UserSettingsUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<UserSettingsUpdateRequest, UserSettings>(
        HttpMethod.Patch,
        SettingsUrl,
        request,
        cancellationToken: cancellationToken
    );
}