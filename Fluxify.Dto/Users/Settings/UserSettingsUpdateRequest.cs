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
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Common;
using Fluxify.Dto.Packs;
using Fluxify.Dto.Users.Relationships;

namespace Fluxify.Dto.Users.Settings;

public record UserSettingsUpdateRequest(
    long? AfkTimeout = null,
    bool? AnimateEmoji = null,
    StickerAnimationOptions? AnimateStickers = null,
    bool? BotDefaultGuildsRestricted = null,
    Snowflake[]? BotRestrictedGuilds = null,
    CustomStatus? CustomStatus = null,
    bool? DefaultGuildsRestricted = null,
    bool? DefaultHideMutedChannels = null,
    bool? DeveloperMode = null,
    FriendSourceFlags? FriendSourceFlags = null,
    bool? GifAutoPlay = null,
    GroupDmAddPermissionFlags? GroupDmAddPermissionFlags = null,
    UserSettingsResponseGuildFoldersItem[]? GuildFolders = null,
    IncomingCallFlags? IncomingCallFlags = null,
    bool? InlineAttachmentMedia = null,
    bool? InlineEmbedMedia = null,
    Locale? Locale = null,
    bool? MessageDisplayCompact = null,
    bool? RenderEmbeds = null,
    bool? RenderReactions = null,
    bool? RenderSpoilers = null,
    Snowflake[]? RestrictedGuilds = null,
    string? Status = null,
    DateTimeOffset? StatusResetsAt = null,
    string? StatusResetsTo = null,
    string? Theme = null,
    TimeFormatTypes? TimeFormat = null,
    string[]? TrustedDomains = null
);