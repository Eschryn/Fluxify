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

[Flags]
[JsonConverter(typeof(PermissionConverter))]
public enum Permissions : ulong
{
    None = 0,
    CreateInstantInvite = 0x1,
    KickMembers = 0x2,
    BanMembers = 0x4,
    Administrator = 0x8,
    ManageChannels = 0x10,
    ManageGuild = 0x20,
    AddReactions = 0x40,
    ViewAuditLog = 0x80,
    PrioritySpeaker = 0x100,
    Stream = 0x200,
    ViewChannel = 0x400,
    SendMessages = 0x800,
    SendTtsMessages = 0x1000,
    ManageMessages = 0x2000,
    EmbedLinks = 0x4000,
    AttachFiles = 0x8000,
    ReadMessageHistory = 0x10000,
    MentionEveryone = 0x20000,
    UseExternalEmojis = 0x40000,
    Connect = 0x100000,
    Speak = 0x200000,
    MuteMembers = 0x400000,
    DeafenMembers = 0x800000,
    MoveMembers = 0x1000000,
    UseVad = 0x2000000,
    ChangeNickname = 0x4000000,
    ManageNicknames = 0x8000000,
    ManageRoles = 0x10000000,
    ManageWebhooks = 0x20000000,
    ManageExpressions = 0x40000000,
    UseExternalStickers = 0x2000000000,
    ModerateMembers = 0x10000000000,
    CreateExpressions = 0x80000000000,
    PinMessages = 0x8000000000000,
    BypassSlowmode = 0x10000000000000,
    UpdateRtcRegion = 0x20000000000000,
}