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

using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.AuditLog;
using Fluxify.Dto.SavedMedia;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.GuildSettings;
using Fluxify.Dto.Users.Relationships;
using Fluxify.Dto.Users.Settings;
using Fluxify.Gateway.Model.Data;
using Fluxify.Gateway.Model.Data.Channel;
using Fluxify.Gateway.Model.Data.Channel.Message;
using Fluxify.Gateway.Model.Data.Channel.Reaction;
using Fluxify.Gateway.Model.Data.Guild;
using Fluxify.Gateway.Model.Data.Guild.Roles;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;

namespace Fluxify.Gateway;

public interface IGatewayClient
{
    string? SessionId { get; }
    ConnectionState ConnectionState { get; }
    Task RunAsync(Uri endpoint, CancellationToken cancellationToken = default);

    Task UpdatePresenceAsync(
        UserStatus status,
        bool? afk = null,
        bool? mobile = null,
        CustomStatus? customStatus = null,
        CancellationToken cancellationToken = default
    );

    Task UpdateVoiceStateAsync(UpdateVoiceState data, CancellationToken cancellationToken = default);

    Task RequestPresenceCountAsync(Snowflake[] guildIds, string? nonce = null,
        CancellationToken cancellationToken = default);

    Task<GuildMemberCount[]> GetPresenceCountAsync(Snowflake[] guildIds,
        CancellationToken cancellationToken = default);
    
    event Action<ConnectionState>? ConnectionStateChanged;
    event Func<ReadyPayload, Task>? Ready;
    event Func<Task>? Resumed;
    event Func<GatewaySession[], Task>? SessionsReplace;
    event Func<UserPrivateReponse, Task>? UserUpdate;
    event Func<Snowflake[], Task>? UserPinnedDmsUpdate;
    event Func<UserSettings, Task>? UserSettingsUpdate;
    event Func<UserGuildSettingsResponse, Task>? UserGuildSettingsUpdate;
    event Func<GatewayUserNoteUpdate, Task>? UserNoteUpdate;
    event Func<GatewayMessageIdResponse, Task>? RecentMentionDelete;
    event Func<MessageResponse, Task>? SavedMessageCreate;
    event Func<GatewayMessageIdResponse, Task>? SavedMessageDelete;
    event Func<FavoriteMemeResponse, Task>? FavoriteMemeCreate;
    event Func<FavoriteMemeResponse, Task>? FavoriteMemeUpdate;
    event Func<GatewayMemeIdResponse, Task>? FavoriteMemeDelete;
    event Func<GatewayAuthSessionChange, Task>? AuthSessionChange;
    event Func<PresenceResponse, Task>? PresenceUpdate;
    event Func<GuildAuditLogEntryResponse, Task>? GuildAuditLogEntryCreate;
    event Func<GatewayGuildCreate, Task>? GuildCreate;
    event Func<GuildResponse, Task>? GuildUpdate;
    event Func<GatewayGuildDelete, Task>? GuildDelete;
    event Func<GatewayGuildMember, Task>? GuildMemberAdd;
    event Func<GatewayGuildMember, Task>? GuildMemberUpdate;
    event Func<GatewayGuildMemberDelete, Task>? GuildMemberRemove;
    event Func<GatewayGuildRole, Task>? GuildRoleCreate;
    event Func<GatewayGuildRole, Task>? GuildRoleUpdate;
    event Func<GatewayGuildRoleDelete, Task>? GuildRoleDelete;
    event Func<GatewayGuildRoleBulk, Task>? GuildRoleUpdateBulk;
    event Func<GatewayEmojiUpdate, Task>? GuildEmojisUpdate;
    event Func<GatewayStickerUpdate, Task>? GuildStickersUpdate;
    event Func<GatewayBanData, Task>? GuildBanAdd;
    event Func<GatewayBanData, Task>? GuildBanRemove;
    event Func<ChannelResponse, Task>? ChannelCreate;
    event Func<ChannelResponse, Task>? ChannelUpdate;
    event Func<GatewayBulkChannelUpdate, Task>? ChannelUpdateBulk;
    event Func<ChannelResponse, Task>? ChannelDelete;
    event Func<GatewayChannelPinsUpdate, Task>? ChannelPinsUpdate;
    event Func<GatewayChannelPinsAck, Task>? ChannelPinsAck;
    event Func<GatewayGroupChange, Task>? ChannelRecipientAdd;
    event Func<GatewayGroupChange, Task>? ChannelRecipientRemove;
    event Func<GatewayMessage, Task>? MessageCreate;
    event Func<GatewayMessage, Task>? MessageUpdate;
    event Func<GatewayMessageDelete, Task>? MessageDelete;
    event Func<GatewayMessageDeleteBulk, Task>? MessageDeleteBulk;
    event Func<GatewayReaction, Task>? MessageReactionAdd;
    event Func<GatewayReaction, Task>? MessageReactionRemove;
    event Func<GatewayReactionRemoveAll, Task>? MessageReactionRemoveAll;
    event Func<GatewayReactionRemoveEmoji, Task>? MessageReactionRemoveEmoji;
    event Func<GatewayMessageAck, Task>? MessageAck;
    event Func<GatewayTypingStart, Task>? TypingStart;
    event Func<GatewayChannelId, Task>? WebhooksUpdate;
    event Func<GatewayInviteDelete, Task>? InviteDelete;
    event Func<RelationshipResponse, Task>? RelationshipAdd;
    event Func<RelationshipResponse, Task>? RelationshipUpdate;
    event Func<GatewayRelationshipId, Task>? RelationshipRemove;
    event Func<VoiceStateResponse, Task>? VoiceStateUpdate;
    event Func<GatewayVoiceServer, Task>? VoiceServerUpdate;
    event Func<GatewayCallSchema, Task>? CallCreate;
    event Func<GatewayCallSchema, Task>? CallUpdate;
    event Func<GatewayChannelId, Task>? CallDelete;

    /// <remarks>
    /// This is only called for users and not bots
    /// </remarks>
    event Func<GatewayPassiveUpdate, Task>? PassiveUpdates;

    event Func<GuildMemberCountsUpdate, Task>? GuildMemberCountUpdate;
}