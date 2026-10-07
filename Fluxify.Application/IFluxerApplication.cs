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
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.EventArgs;
using Fluxify.Gateway;
using Fluxify.Rest;

namespace Fluxify.Application;

/// <summary>
/// 
/// </summary>
public interface IFluxerApplication
{
    /// <summary>
    /// Fluxer gateway client.
    /// </summary>
    IGatewayClient Gateway { get; }
    
    /// <summary>
    /// Fluxer REST client.
    /// </summary>
    RestClient Rest { get; }
    
    /// <summary>
    /// Gets the current user.
    /// </summary>
    PrivateUser CurrentUser { get; }
    
    /// <summary>
    /// Gets all cached guilds.
    /// </summary>
    IReadOnlyCollection<CacheRef<Guild>> Guilds { get; }
    
    /// <summary>
    /// Gets all cached private text channels.
    /// </summary>
    IReadOnlyCollection<PrivateTextChannel> PrivateChannels { get; }

    /// <summary>
    /// Starts the bot.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to exit the bot.</param>
    /// <remarks>
    /// This method runs until canceled.
    /// </remarks>
    Task RunAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets or creates a direct message channel with the specified user.
    /// </summary>
    /// <param name="userId">The id of the user, for which the direct message channel should be retrieved or created.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Snapshot of the direct message channel entity.</returns>
    Task<Dm> GetOrCreateDmAsync(Snowflake userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a guild.
    /// </summary>
    /// <param name="guildId">The id of the guild that should be retrieved.</param>
    /// <param name="bypassCache">If set to true, it will disable the cache for this request.</param>
    /// <returns>Snapshot of the guild entity.</returns>
    Task<Guild> GetGuildAsync(
        Snowflake guildId,
        bool bypassCache = false
    );

    /// <summary>
    /// Gets a channel.
    /// </summary>
    /// <param name="channelId">The id of the channel that should be retrieved.</param>
    /// <param name="bypassCache">If set to true, it will disable the cache for this request.</param>
    /// <returns>Snapshot of the channel entity.</returns>
    Task<IChannel> GetChannelAsync(
        Snowflake channelId,
        bool bypassCache = false
    );

    /// <summary>
    /// Gets a user.
    /// </summary>
    /// <param name="userId">The id of the user that should be retrieved.</param>
    /// <param name="bypassCache">If set to true, it will disable the cache for this request.</param>
    /// <returns>Snapshot of the user entity.</returns>
    Task<GlobalUser> GetUserAsync(
        Snowflake userId,
        bool bypassCache = false
    );

    /// <summary>
    /// Fired when a message was received from a channel.
    /// </summary>
    event AsyncEventHandler<MessageEventArgs> MessageReceived;

    /// <summary>
    /// Fired when a message was received in a channel.
    /// </summary>
    event AsyncEventHandler<MessageEventArgs> MessageUpdated;

    /// <summary>
    /// Fired when a message was deleted in a channel.
    /// </summary>
    event AsyncEventHandler<MessageDeletedEventArgs> MessageDeleted;

    /// <summary>
    /// Fired when multiple messages were deleted in a channel.
    /// </summary>
    event AsyncEventHandler<MessagesBulkDeletedEventArgs> MessageBulkDeleted;

    /// <summary>
    /// Fired when a reaction was added to a message.
    /// </summary>
    event AsyncEventHandler<ReactionEventArgs> MessageReactionAdd;

    /// <summary>
    /// Fired when a reaction was removed from a message.
    /// </summary>
    event AsyncEventHandler<ReactionEventArgs> MessageReactionRemove;

    /// <summary>
    /// Fired when all reactions for an emoji were removed from a message.
    /// </summary>
    event AsyncEventHandler<ReactionRemoveEmojiEventArgs> MessageReactionRemoveEmoji;

    /// <summary>
    /// Fired when all reactions were removed from a message.
    /// </summary>
    event AsyncEventHandler<ReactionRemoveAllEventArgs> MessageReactionRemoveAll;

    /// <summary>
    /// Fired when a guild became available.
    /// </summary>
    /// <remarks>
    ///This can either happen after an outage, joining a server, creating a server or when logged in using a bot account it happens during the startup.
    /// </remarks>
    event AsyncEventHandler<GuildEventArgs> GuildCreated;

    /// <summary>
    /// Fired when a guild was updated.
    /// </summary>
    event AsyncEventHandler<GuildEventArgs> GuildUpdated;

    /// <summary>
    /// Fired when a guild became unavailable.
    /// </summary>
    /// <remarks>
    /// This can either happen due to an outage, a user being removed from the guild or the guild was deleted.
    /// </remarks>
    event AsyncEventHandler<GuildDeletedEventArgs> GuildDeleted;

    /// <summary>
    /// Fired when a channel was created.
    /// </summary>
    event AsyncEventHandler<ChannelEventArgs> ChannelCreated;

    /// <summary>
    /// Fired when a channel was updated.
    /// </summary>
    event AsyncEventHandler<ChannelUpdatedEventArgs> ChannelUpdated;

    /// <summary>
    /// Fired when a channel was deleted.
    /// </summary>
    event AsyncEventHandler<ChannelEventArgs> ChannelDeleted;

    /// <summary>
    /// Fired when a role was created.
    /// </summary>
    event AsyncEventHandler<GuildRoleEventArgs> RoleCreated;

    /// <summary>
    /// Fired when a role was updated.
    /// </summary>
    event AsyncEventHandler<GuildRoleEventArgs> RoleUpdated;

    /// <summary>
    /// Fired when a role was deleted.
    /// </summary>
    event AsyncEventHandler<GuildRoleEventArgs> RoleDeleted;

    /// <summary>
    /// Fired when multiple roles at once were updated.
    /// </summary>
    event AsyncEventHandler<GuildRolesEventArgs> RoleUpdatedBulk;

    /// <summary>
    /// Fired when a member joins a guild.
    /// </summary>
    event AsyncEventHandler<GuildMemberEventArgs> GuildMemberAdded;

    /// <summary>
    /// Fired when a member updates their member profile or a moderator performed specific moderation actions.
    /// </summary>
    event AsyncEventHandler<GuildMemberEventArgs> GuildMemberUpdated;

    /// <summary>
    /// Fired when a member leaves a guild, due to leaving or being kicked/banned.
    /// </summary>
    event AsyncEventHandler<GuildMemberEventArgs> GuildMemberRemoved;

    /// <summary>
    /// Fired when a user gets banned in a guild.
    /// </summary>
    event AsyncEventHandler<GuildBanEventArgs> GuildBanAdd;

    /// <summary>
    /// Fired when a user gets unbanned in a guild.
    /// </summary>
    event AsyncEventHandler<GuildBanEventArgs> GuildBanRemove;
    
    /// <summary>
    /// Fired when a new member leaves a group or is removed from a group.
    /// </summary>
    event AsyncEventHandler<GroupMembershipEventArgs> GroupMemberRemoved;
    
    /// <summary>
    /// Fired when a new member is added to a group or joined via group invitation.
    /// </summary>
    event AsyncEventHandler<GroupMembershipEventArgs> GroupMemberAdded;

    /*
    event AsyncEventHandler<ReadyPayload> Ready;
    event Func<Task>? Resumed;
    event Func<GatewaySession[], Task>? SessionsReplace;
    event AsyncEventHandler<UserPrivateResponse> UserUpdate;
    event Func<Snowflake[], Task>? UserPinnedDmsUpdate;
    event AsyncEventHandler<UserSettings> UserSettingsUpdate;
    event AsyncEventHandler<UserGuildSettingsResponse> UserGuildSettingsUpdate;
    event AsyncEventHandler<GatewayUserNoteUpdate> UserNoteUpdate;
    event AsyncEventHandler<GatewayMessageIdResponse> RecentMentionDelete;
    event AsyncEventHandler<MessageResponse> SavedMessageCreate;
    event AsyncEventHandler<GatewayMessageIdResponse> SavedMessageDelete;
    event AsyncEventHandler<FavoriteMemeResponse> FavoriteMemeCreate;
    event AsyncEventHandler<FavoriteMemeResponse> FavoriteMemeUpdate;
    event AsyncEventHandler<GatewayMemeIdResponse> FavoriteMemeDelete;
    event AsyncEventHandler<GatewayAuthSessionChange> AuthSessionChange;
    event AsyncEventHandler<PresenceResponse> PresenceUpdate;
    event AsyncEventHandler<GuildAuditLogEntryResponse> GuildAuditLogEntryCreate;
    event AsyncEventHandler<GatewayEmojiUpdate> GuildEmojisUpdate;
    event AsyncEventHandler<GatewayStickerUpdate> GuildStickersUpdate;
    event AsyncEventHandler<GatewayBulkChannelUpdate> ChannelUpdateBulk;
    event AsyncEventHandler<GatewayChannelPinsUpdate> ChannelPinsUpdate;
    event AsyncEventHandler<GatewayChannelPinsAck> ChannelPinsAck;
    event AsyncEventHandler<GatewayMessageAck> MessageAck;
    event AsyncEventHandler<GatewayTypingStart> TypingStart;
    event AsyncEventHandler<GatewayChannelId> WebhooksUpdate;
    event AsyncEventHandler<GatewayInviteDelete> InviteDelete;
    event AsyncEventHandler<RelationshipResponse> RelationshipAdd;
    event AsyncEventHandler<RelationshipResponse> RelationshipUpdate;
    event AsyncEventHandler<GatewayRelationshipId> RelationshipRemove;
    event AsyncEventHandler<VoiceStateResponse> VoiceStateUpdate;
    event AsyncEventHandler<GatewayVoiceServer> VoiceServerUpdate;
    event AsyncEventHandler<GatewayCallSchema> CallCreate;
    event AsyncEventHandler<GatewayCallSchema> CallUpdate;
    event AsyncEventHandler<GatewayChannelId> CallDelete;

    /// <remarks>
    /// This is only called for users and not bots
    /// </remarks>
    event AsyncEventHandler<GatewayPassiveUpdate> PassiveUpdates;*/
}