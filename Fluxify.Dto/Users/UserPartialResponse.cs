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

namespace Fluxify.Dto.Users;

/// <summary>
/// Represents the publicly visible part of a user.
/// </summary>
/// <param name="Avatar">The profile picture media hash.</param>
/// <param name="AvatarColor">The derived color of the profile picture. </param>
/// <param name="Discriminator">The four digit discriminator of the user handle.</param>
/// <param name="Flags">Publicly known attributes of this user.</param>
/// <param name="GlobalName">The global display name of the user.</param>
/// <param name="Id">The id of the user.</param>
/// <param name="System">True when the user is a system account, otherwise false.</param>
/// <param name="Bot">True when the user is a bot account, otherwise false.</param>
/// <param name="Username">The username part of the user handle.</param>
public record UserPartialResponse(
    MediaHash? Avatar,
    Color? AvatarColor,
    string Discriminator,
    PublicUserFlags Flags,
    string? GlobalName,
    Snowflake Id,
    bool? System,
    bool? Bot,
    string Username)
{
    /// <summary>The four digit discriminator of the user handle.</summary>
    /// <remarks>
    /// Valid range: #0000-#9999.<br/>
    /// #0000 is reserved for visionary members.<br/>
    /// #0001 is reserved for plutonium members.
    /// </remarks>
    public string Discriminator { get; init; } = Discriminator;
}