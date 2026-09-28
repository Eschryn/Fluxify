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

using System.Drawing;
using Fluxify.Core.Types;
using Fluxify.Dto.Common;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Members;

/// <summary>
/// Represents a guild member.
/// </summary>
/// <param name="AccentColor">The accent color that is displayed in the UI as background/border.</param>
/// <param name="Avatar">The guild specific profile picture hash of the user. Is null when no override exists/global profile picture applies.</param>
/// <param name="Banner">The guild specific banner image hash of the user. Is null when no override exists/global banner image applies.</param>
/// <param name="JoinedAt">The date that the user joined the guild.</param>
/// <param name="CommunicationsDisabledUntil">Set when the user has a timeout set. They cannot communicate until this timestamp passed.</param>
/// <param name="Deaf">True when the user has been guild wide deafened.</param>
/// <param name="Mute">True when the user has been guild wide muted.</param>
/// <param name="Nick">The guild specific nickname of the user.</param>
/// <param name="ProfileFlags">Attributes of the guild member.</param>
/// <param name="MentionFlags">Tells if a guild member has preference in whether to mention or not to mention them.</param>
/// <param name="Roles">The role ids of the roles that the member has been assigned to.</param>
/// <param name="User">The corresponding public user for this guild member.</param>
public record GuildMemberResponse(
    Color? AccentColor,
    MediaHash? Avatar,
    MediaHash? Banner,
    DateTimeOffset? JoinedAt,
    DateTimeOffset? CommunicationsDisabledUntil,
    bool Deaf,
    bool Mute,
    string? Nick,
    GuildMemberProfileFlags ProfileFlags,
    MentionFlags MentionFlags,
    Snowflake[] Roles,
    UserPartialResponse? User
);