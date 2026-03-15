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

namespace Fluxify.Dto.Guilds.AuditLog;

/// <summary>
/// Audit log event types
/// </summary>
public enum AuditLogActionType
{
    /// <summary>
    /// The settings of the guild were updated
    /// </summary>
    GuildSettingsUpdate = 1,

    /// <summary>
    /// A new channel was created
    /// </summary>
    ChannelCreated = 10,

    /// <summary>
    /// A channel was updated
    /// </summary>
    ChannelUpdated = 11,

    /// <summary>
    /// A channel was deleted
    /// </summary>
    ChannelDeleted = 12,

    /// <summary>
    /// A permission overwrite was created
    /// </summary>
    PermissionOverwriteCreated = 13,

    /// <summary>
    /// A permission overwrite was updated
    /// </summary>
    PermissionOverwriteUpdated = 14,

    /// <summary>
    /// A permission overwrite was deleted
    /// </summary>
    PermissionOverwriteDeleted = 15,

    /// <summary>
    /// A member was kicked
    /// </summary>
    MemberKicked = 20,

    /// <summary>
    /// Members were pruned
    /// </summary>
    MembersPruned = 21,

    /// <summary>
    /// A member was banned
    /// </summary>
    MemberBanned = 22,

    /// <summary>
    /// A member's ban was removed
    /// </summary>
    MemberUnbanned = 23,

    /// <summary>
    /// A member was updated
    /// </summary>
    MemberUpdated = 24,

    /// <summary>
    /// A member's roles were updated
    /// </summary>
    MemberRolesUpdated = 25,

    /// <summary>
    /// A member was moved to a different voice channel
    /// </summary>
    VoiceMemberMoved = 26,

    /// <summary>
    /// A member was disconnected from a voice channel
    /// </summary>
    VoiceMemberDisconnected = 27,

    /// <summary>
    /// A bot was added to the guild
    /// </summary>
    BotAdded = 28,

    /// <summary>
    /// A role was created
    /// </summary>
    RoleCreated = 30,

    /// <summary>
    /// A role was updated
    /// </summary>
    RoleUpdated = 31,

    /// <summary>
    /// A role was deleted
    /// </summary>
    RoleDeleted = 32,

    /// <summary>
    /// An invite link was created
    /// </summary>
    InviteCreated = 40,

    /// <summary>
    /// An invite link was updated
    /// </summary>
    InviteUpdated = 41,

    /// <summary>
    /// An invite link was deleted
    /// </summary>
    InviteDeleted = 42,

    /// <summary>
    /// A webhook link was created
    /// </summary>
    WebhookCreated = 50,

    /// <summary>
    /// A webhook link was updated
    /// </summary>
    WebhookUpdated = 51,

    /// <summary>
    /// A webhook link was deleted
    /// </summary>
    WebhookDeleted = 52,

    /// <summary>
    /// An emoji was added to the guild
    /// </summary>
    EmojiCreated = 60,

    /// <summary>
    /// An emoji was updated
    /// </summary>
    EmojiUpdated = 61,

    /// <summary>
    /// An emoji was deleted from the guild
    /// </summary>
    EmojiDeleted = 62,

    /// <summary>
    /// A sticker was added to the guild
    /// </summary>
    StickerCreated = 90,

    /// <summary>
    /// A sticker was updated
    /// </summary>
    StickerUpdated = 91,

    /// <summary>
    /// A sticker was deleted from the guild
    /// </summary>
    StickerDeleted = 92,

    /// <summary>
    /// A message has been deleted
    /// </summary>
    MessageDeleted = 72,

    /// <summary>
    /// Messages have been bulk deleted
    /// </summary>
    MessagesBulkDeleted = 73,

    /// <summary>
    /// A message was pinned to a channel
    /// </summary>
    MessagePinned = 74,

    /// <summary>
    /// A message was unpinned from a channel
    /// </summary>
    MessageUnpinned = 75
}