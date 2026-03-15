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

namespace Fluxify.Dto.Guilds.Roles;

/// <summary>
/// 
/// </summary>
/// <param name="Color"></param>
/// <param name="Hoist"> Role is displayed seperately in member list</param>
/// <param name="???"></param>
public record GuildRoleResponse(
    int Color,
    bool Hoist,
    long? HoistPosition,
    Snowflake Id,
    bool Mentionable,
    string Name,
    Permissions Permissions,
    long Position,
    string? UnicodeEmoji
);