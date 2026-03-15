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

namespace Fluxify.Gateway.Model;

public static class GatewayEvent
{
    /// <summary>
    /// Emitted by the server when the client has successfully identified with the gateway
    /// </summary>
    public const string Ready = "READY";
    /// <summary>
    /// Emitted by the server when a previous session has been successfully restored
    /// </summary>
    public const string Resumed = "RESUMED";
    public const string SessionsReplace = "SESSIONS_REPLACE";
    public const string UserUpdate = "USER_UPDATE";
    public const string UserPinnedDmsUpdate = "USER_PINNED_DMS_UPDATE";
    public const string UserSettingsUpdate = "USER_SETTINGS_UPDATE";
    public const string UserGuildSettingsUpdate = "USER_GUILD_SETTINGS_UPDATE";
    public const string UserNoteUpdate = "USER_NOTE_UPDATE";
    public const string RecentMentionDelete = "RECENT_MENTION_DELETE";
    public const string SavedMessageCreate = "SAVED_MESSAGE_CREATE";
    public const string SavedMessageDelete = "SAVED_MESSAGE_DELETE";
    public const string FavoriteMemeCreate = "FAVORITE_MEME_CREATE";
    public const string FavoriteMemeUpdate = "FAVORITE_MEME_UPDATE";
    public const string FavoriteMemeDelete = "FAVORITE_MEME_DELETE";
    public const string AuthSessionChange = "AUTH_SESSION_CHANGE";
    public const string PresenceUpdate = "PRESENCE_UPDATE";
    public const string GuildCreate = "GUILD_CREATE";
    public const string GuildUpdate = "GUILD_UPDATE";
    public const string GuildDelete = "GUILD_DELETE";
    public const string GuildMemberAdd = "GUILD_MEMBER_ADD";
    public const string GuildMemberUpdate = "GUILD_MEMBER_UPDATE";
    public const string GuildMemberRemove = "GUILD_MEMBER_REMOVE";
    public const string GuildRoleCreate = "GUILD_ROLE_CREATE";
    public const string GuildRoleUpdate = "GUILD_ROLE_UPDATE";
    public const string GuildRoleUpdateBulk = "GUILD_ROLE_UPDATE_BULK";
    public const string GuildRoleDelete = "GUILD_ROLE_DELETE";
    public const string GuildEmojisUpdate = "GUILD_EMOJIS_UPDATE";
    public const string GuildStickersUpdate = "GUILD_STICKERS_UPDATE";
    public const string GuildBanAdd = "GUILD_BAN_ADD";
    public const string GuildBanRemove = "GUILD_BAN_REMOVE";
    public const string ChannelCreate = "CHANNEL_CREATE";
    public const string ChannelUpdate = "CHANNEL_UPDATE";
    public const string ChannelUpdateBulk = "CHANNEL_UPDATE_BULK";
    public const string ChannelDelete = "CHANNEL_DELETE";
    public const string ChannelPinsUpdate = "CHANNEL_PINS_UPDATE";
    public const string ChannelPinsAck = "CHANNEL_PINS_ACK";
    public const string ChannelRecipientAdd = "CHANNEL_RECIPIENT_ADD";
    public const string ChannelRecipientRemove = "CHANNEL_RECIPIENT_REMOVE";
    public const string MessageCreate = "MESSAGE_CREATE";
    public const string MessageUpdate = "MESSAGE_UPDATE";
    public const string MessageDelete = "MESSAGE_DELETE";
    public const string MessageDeleteBulk = "MESSAGE_DELETE_BULK";
    public const string MessageReactionAdd = "MESSAGE_REACTION_ADD";
    public const string MessageReactionRemove = "MESSAGE_REACTION_REMOVE";
    public const string MessageReactionRemoveAll = "MESSAGE_REACTION_REMOVE_ALL";
    public const string MessageReactionRemoveEmoji = "MESSAGE_REACTION_REMOVE_EMOJI";
    public const string MessageAck = "MESSAGE_ACK";
    public const string TypingStart = "TYPING_START";
    public const string WebhooksUpdate = "WEBHOOKS_UPDATE";
    public const string InviteCreate = "INVITE_CREATE";
    public const string InviteDelete = "INVITE_DELETE";
    public const string RelationshipAdd = "RELATIONSHIP_ADD";
    public const string RelationshipUpdate = "RELATIONSHIP_UPDATE";
    public const string RelationshipRemove = "RELATIONSHIP_REMOVE";
    public const string VoiceStateUpdate = "VOICE_STATE_UPDATE";
    public const string VoiceServerUpdate = "VOICE_SERVER_UPDATE";
    public const string CallCreate = "CALL_CREATE";
    public const string CallUpdate = "CALL_UPDATE";
    public const string CallDelete = "CALL_DELETE";
}