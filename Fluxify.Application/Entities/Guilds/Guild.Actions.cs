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

using Fluxify.Application.Common;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Invites;
using Fluxify.Application.Entities.Webhooks;
using Fluxify.Application.Model.AuditLog;
using Fluxify.Application.Model.Channel;
using Fluxify.Application.Model.Guild;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Guilds.AuditLog;
using Fluxify.Dto.Guilds.Invite;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Settings;
using Fluxify.Dto.Users.Settings.Security;

namespace Fluxify.Application.Entities.Guilds;

public partial class Guild
{
    public Task<GuildTextChannel> CreateTextChannelAsync(
        string name,
        Action<TextChannelProperties>? configure = null,
        CancellationToken cancellationToken = default
    ) => _app.ChannelsRepository.CreateAsync<GuildTextChannel>(Id, new TextChannelProperties
    {
        Name = name
    }.Configure(configure));

    public Task<GuildVoiceChannel> CreateVoiceChannelAsync(
        string name,
        Action<VoiceChannelProperties>? configure = null
    ) => _app.ChannelsRepository.CreateAsync<GuildVoiceChannel>(Id, new VoiceChannelProperties
    {
        Name = name
    }.Configure(configure));

    public Task<GuildLinkChannel> CreateLinkChannelAsync(
        string name,
        string url,
        Action<LinkChannelProperties>? configure = null
    ) => _app.ChannelsRepository.CreateAsync<GuildLinkChannel>(Id, new LinkChannelProperties
    {
        Name = name,
        Url = url
    }.Configure(configure));

    public Task<GuildCategory> CreateCategoryAsync(
        string name,
        Action<CategoryProperties>? configure = null
    ) => _app.ChannelsRepository.CreateAsync<GuildCategory>(Id, new CategoryProperties
    {
        Name = name
    }.Configure(configure));

    public async Task<Webhook[]> GetWebhooksAsync(
        CancellationToken cancellationToken = default
    ) => (await RequestBuilder.GetWebhooksAsync(cancellationToken))
        .Select(_app.WebhookMapper.FromResponse)
        .ToArray();

    public async Task<Webhook> GetWebhookAsync(
        Snowflake id,
        CancellationToken cancellationToken = default
    ) => _app.WebhookMapper.FromResponse(
        await _app.Rest.Webhooks[id].GetAsync(cancellationToken) ?? throw new Exception("Webhook was not found"));

    public Task<IGuildMember?> GetMemberAsync(Snowflake id)
        => MembersRepository.GetAsync(id)
            .ContinueWith(t => t.Result.Value, TaskContinuationOptions.OnlyOnRanToCompletion);

    public Task BanAsync(
        Snowflake userId,
        int? deleteMessageDays = null,
        TimeSpan? banDuration = null,
        string? banReason = null,
        string? auditLogReason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.BanAsync(
        userId,
        new GuildBanCreateRequest(
            (long?)banDuration?.TotalSeconds,
            deleteMessageDays,
            banReason
        ),
        auditLogReason,
        cancellationToken
    );

    public Task UnbanAsync(
        Snowflake userId,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.UnbanAsync(
        userId,
        reason,
        cancellationToken
    );

    public async Task<IGuildChannel?> GetChannelAsync(
        Snowflake id,
        bool bypassCache = false,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _app.ChannelsRepository.GetAsync(id, bypassCache);

        return result.Value as IGuildChannel;
    }

    public async Task<GuildChannelInviteMetadata[]?> GetInvitesAsync(
        CancellationToken cancellationToken = default
    )
    {
        var invites = await RequestBuilder.ListInvitesAsync(cancellationToken);

        return invites
            .Select(_app.InviteMapper.MapFromResponse)
            .OfType<GuildChannelInviteMetadata>()
            .ToArray();
    }

    public Task<Guild> UpdateAsync(
        SudoVerificationSchema verificationSchema,
        Action<GuildProperties> update,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => _app.GuildsRepository.UpdateAsync(this, verificationSchema, update, reason, cancellationToken);
    
    public Task DeleteAsync(
        SudoVerificationSchema verificationSchema,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => _app.GuildsRepository.DeleteAsync(Id, verificationSchema, reason, cancellationToken);

    public Task LeaveAsync(
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => _app.GuildsRepository.LeaveAsync(Id, reason, cancellationToken);

    public Task TransferOwnership(
        Snowflake newOwnerId,
        SudoVerificationSchema verificationSchema,
        string? reason = null,
        CancellationToken cancellationToken = default)
    {
        if (CurrentMember.Id != Owner.Id)
        {
            throw new InvalidOperationException("You must be the owner of the guild to transfer ownership.");
        }

        return RequestBuilder.TransferOwnershipAsync(
            new GuildTransferOwnershipRequest(
                NewOwnerId: newOwnerId,
                MfaCode: verificationSchema.MfaCode,
                MfaMethod: verificationSchema.MfaMethod,
                Password: verificationSchema.Password,
                WebauthnChallenge: verificationSchema.WebauthnChallenge,
                WebauthnResponse: verificationSchema.WebauthnResponse
            ),
            reason,
            cancellationToken
        );
    }

    public Task GetVanityUrlAsync()
        => RequestBuilder
            .GetVanityUrlAsync()
            .MapAsync(_app.InviteMapper.MapVanity);

    public Task SetVanityUrlAsync(
        string code,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.UpdateVanityUrlAsync(
        new GuildVanityUrlUpdateRequest(
            Code: code
        ),
        reason,
        cancellationToken
    ).MapAsync(_app.InviteMapper.MapVanity);

    public Task MoveChannelAsync(
        IGuildChannel channel,
        long position,
        IGuildChannel? parent = null,
        IGuildChannel? after = null,
        bool? lockPermissions = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.UpdateChannelPositionAsync(
        new ChannelPositionUpdateRequest(
            channel.Id,
            position,
            parent?.Id,
            after?.Id,
            lockPermissions
        ),
        cancellationToken
    );

    public Task<AuditLogEntry[]> GetAuditLogEntriesAsync(
        Snowflake? pageAnchor = null,
        Direction direction = Direction.After,
        int? limit = null,
        Snowflake? byUserId = null,
        AuditLogActionType? eventType = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.ListAuditLogAsync(
            limit,
            direction == Direction.Before ? pageAnchor : null,
            direction == Direction.After ? pageAnchor : null,
            byUserId,
            eventType,
            cancellationToken
        )
        .MapAsync(_app.AuditLogMapper.MapFromResponse);

    public Task<CacheRef<Guild>> SetDetachedBannerAsync(
        bool detached,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.ToggleDetachedBannerAsync(
        new EnabledRequest(detached),
        reason,
        cancellationToken
    ).MapAsync(_app.GuildsRepository.Insert);

    public Task<CacheRef<Guild>> SetFlexibleChannelNamesAsync(
        bool flexibleChannelNames,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.ToggleTextChannelFlexibleNamesAsync(
        new EnabledRequest(flexibleChannelNames),
        reason,
        cancellationToken
    ).MapAsync(_app.GuildsRepository.Insert);

    public Task<CacheRef<Guild>> SetInvitesDisabledAsync(
        bool disabled,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.ToggleInvitesDisabledAsync(
        new EnabledRequest(disabled),
        reason,
        cancellationToken
    ).MapAsync(_app.GuildsRepository.Insert);

    public Task<CacheRef<Guild>> SetCloneEmojiDisabledAsync(
        bool disabled,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.ToggleCloneEmojiDisabledAsync(
        new EnabledRequest(disabled),
        reason,
        cancellationToken
    ).MapAsync(_app.GuildsRepository.Insert);

    public Task<CacheRef<Guild>> SetCloneStickerDisabledAsync(
        bool disabled,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.ToggleCloneStickerDisabledAsync(
        new EnabledRequest(disabled),
        reason,
        cancellationToken
    ).MapAsync(_app.GuildsRepository.Insert);

    public Task<CacheRef<Guild>> SetHideOwnerCrownAsync(
        bool hide,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.ToggleHideOwnerCrownAsync(
        new EnabledRequest(hide),
        reason,
        cancellationToken
    ).MapAsync(_app.GuildsRepository.Insert);
}