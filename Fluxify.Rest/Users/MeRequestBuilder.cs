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
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.GuildSettings;
using Fluxify.Dto.Users.Settings;
using Fluxify.Dto.Users.Settings.Security;

namespace Fluxify.Rest.Users;

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

    public Task<UserPrivateReponse> GetMeAsync(CancellationToken cancellationToken = default)
        => client.JsonRequestAsync<UserPrivateReponse>(
            HttpMethod.Get,
            MeUrl,
            cancellationToken: cancellationToken
        );

    public Task<UserPrivateReponse> UpdateMeAsync(
        UserUpdateWithVerificationRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<UserUpdateWithVerificationRequest, UserPrivateReponse>(
        HttpMethod.Patch,
        MeUrl,
        request,
        cancellationToken: cancellationToken
    );

    public Task ForgetAuthorizedIpAsync(SudoVerificationSchema request,
        CancellationToken cancellationToken = default)
        => client.JsonRequestAsync(
            HttpMethod.Delete,
            AuthorizedIpsUrl,
            request,
            cancellationToken: cancellationToken
        );

    public Task DeleteAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        DeleteUrl,
        request,
        cancellationToken: cancellationToken
    );

    public Task DisableAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        DisableUrl,
        request,
        cancellationToken: cancellationToken
    );

    public Task<GiftCodeMetadataResponse[]> ListGiftsAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GiftCodeMetadataResponse[]>(
        HttpMethod.Get,
        GiftsUrl,
        cancellationToken: cancellationToken
    );

    public Task<UserGuildSettingsResponse> DmNotificationSettings(
        UserGuildSettingsRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<UserGuildSettingsRequest, UserGuildSettingsResponse>(
        HttpMethod.Patch,
        DmSettingsUrl,
        request,
        cancellationToken: cancellationToken
    );

    public Task<UserGuildSettingsResponse> GuildNotificationSettings(
        Snowflake guildId,
        UserGuildSettingsRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<UserGuildSettingsRequest, UserGuildSettingsResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, GuildSettingsUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public Task<MessageResponse[]> ListMentionsAsync(
        int? limit = null,
        bool? roles = null,
        bool? everyone = null,
        bool? guilds = null,
        Snowflake? before = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<MessageResponse[]>(
        HttpMethod.Get,
        MentionsUrl,
        cancellationToken: cancellationToken
    );

    public Task DeleteMentionAsync(
        Snowflake messageId,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, MentionsIdUrl, messageId),
        cancellationToken: cancellationToken
    );

    public Task<Dictionary<Snowflake, MessageResponse>> PreloadMessagesAsync(
        PreloadMessagesRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<PreloadMessagesRequest, Dictionary<Snowflake, MessageResponse>>(
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
    public Task ResetPremiumStatusAsync(CancellationToken cancellationToken = default)
        => client.RequestAsync(
            HttpMethod.Post,
            ResetPremiumUrl,
            cancellationToken: cancellationToken
        );

    public Task<UserSettings> GetUserSettingsAsync(CancellationToken cancellationToken = default)
        => client.JsonRequestAsync<UserSettings>(
            HttpMethod.Get,
            SettingsUrl,
            cancellationToken: cancellationToken
        );

    public Task<UserSettings> UpdateUserSettingsAsync(
        UserSettingsUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<UserSettingsUpdateRequest, UserSettings>(
        HttpMethod.Patch,
        SettingsUrl,
        request,
        cancellationToken: cancellationToken
    );
}