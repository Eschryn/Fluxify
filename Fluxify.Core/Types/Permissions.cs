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

using System.Text.Json.Serialization;

namespace Fluxify.Core.Types;

/// <summary>
/// Provides permission attributes as <see cref="ulong"/> flag enum.
/// </summary>
[Flags]
[JsonConverter(typeof(PermissionConverter))]
public enum Permissions : ulong
{
    /// <summary>
    /// No permission.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Permission for creating an invite to a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    CreateInstantInvite = 1L << 0,
    
    /// <summary>
    /// Permission to kick members from a guild.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    KickMembers = 1L << 1,
    
    /// <summary>
    /// Permission to ban members from a guild.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    BanMembers = 1L << 2,
    
    /// <summary>
    /// All permission and ignores channel overwrites.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    Administrator = 1L << 3,
    
    /// <summary>
    /// Permission to create, modify, reorder and delete channels in the guild.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    ManageChannels = 1L << 4,
    
    /// <summary>
    /// Permission manage guild settings and resources like vanity url, invites and discovery.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    ManageGuild = 1L << 5,
    
    /// <summary>
    /// Permission to add reactions to messages in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    AddReactions = 1L << 6,
    
    /// <summary>
    /// Permission to view the guild audit log.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    ViewAuditLog = 1L << 7,
    
    /// <summary>
    /// Permission to use priority speaker in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    PrioritySpeaker = 1L << 8,
    
    /// <summary>
    /// Permission to screen share in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    Stream = 1L << 9,
    
    /// <summary>
    /// Permission to view/see a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    ViewChannel = 1L << 10,
    
    /// <summary>
    /// Permission to send messages in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    SendMessages = 1L << 11,
    
    /// <summary>
    /// Permission to send tts messages in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    SendTtsMessages = 1L << 12,
    
    /// <summary>
    /// Permission to (bulk-) delete messages of another member in a guild channel, modify the message flags and attachments and remove reactions from messages. 
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    ManageMessages = 1L << 13,
    
    /// <summary>
    /// Permission to have links expanded into embeds in the authors message.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    EmbedLinks = 1L << 14,
    
    /// <summary>
    /// Permission to attach files to a message in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    AttachFiles = 1L << 15,
    
    /// <summary>
    /// Permission to read the message history of a channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    ReadMessageHistory = 1L << 16,
    
    /// <summary>
    /// Permission to use @everyone and @here in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    MentionEveryone = 1L << 17,
    
    /// <summary>
    /// Permission to use external (from another guild) emojis in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    UseExternalEmojis = 1L << 18,
    
    /// <summary>
    /// Permission to connect to a guild voice channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    Connect = 1L << 20,
    
    /// <summary>
    /// Permission to transmit audio in a guild voice channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    Speak = 1L << 21,
    
    /// <summary>
    /// Permission to guild wide mute members of a guild.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    MuteMembers = 1L << 22,
    
    /// <summary>
    /// Permission to guild wide deafen members of a guild.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    DeafenMembers = 1L << 23,
    
    /// <summary>
    /// Permission to move members between guild voice channels.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    MoveMembers = 1L << 24,
    
    /// <summary>
    /// Permission to use voice activity detection rather than push to talk in a guild voice channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    UseVad = 1L << 25,
    
    /// <summary>
    /// Permission to change your own nickname for a guild.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    ChangeNickname = 1L << 26,
    
    /// <summary>
    /// Permission to modify nicknames of other members in a guild.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    ManageNicknames = 1L << 27,
    
    /// <summary>
    /// Permission to create, modify, delete roles, change their position, assign and remove roles from and to members in a guild and set channel permission overwrites.
    /// </summary>
    /// <remarks>Scope: guild, channel</remarks>
    ManageRoles = 1L << 28,
    
    /// <summary>
    /// Permission to create, modify or delete webhooks for a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    ManageWebhooks = 1L << 29,
    
    /// <summary>
    /// Permission to modify or delete stickers and emojis created by another member in a guild.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    ManageExpressions = 1L << 30,
    
    /// <summary>
    /// Permission to use external (from another guild) stickers in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    UseExternalStickers = 1L << 37,
    
    /// <summary>
    /// Apply and clear a communication timeout.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    ModerateMembers = 1L << 40,
    
    /// <summary>
    /// Permission to create stickers and emojis in a guild, and modify and delete those that were created by the caller.
    /// </summary>
    /// <remarks>Scope: guild</remarks>
    CreateExpressions = 1L << 43,
    
    /// <summary>
    /// Permission to pin and unpin messages in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    PinMessages = 1L << 51,
    
    /// <summary>
    /// Permission to bypass slow mode in a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    BypassSlowmode = 1L << 52,
    
    /// <summary>
    /// Permission to change the voice region of a guild voice channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    UpdateRtcRegion = 1L << 53,
    
    /// <summary>
    /// Permission to view the members of a guild channel.
    /// </summary>
    /// <remarks>Scope: channel</remarks>
    ViewChannelMembers = 1L << 54
}