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

using System.Diagnostics.CodeAnalysis;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;

namespace Fluxify.Application.Model.Channel;

[method: SetsRequiredMembers]
public abstract class PermissionOverwrite(Snowflake id, PermissionOverwriteType type)
{
    public required Snowflake Id { get; init; } = id;
    public required PermissionOverwriteType Type { get; init; } = type;
    public Permissions? Allow { get; set; }
    public Permissions? Deny { get; set; }
    
    [method: SetsRequiredMembers]
    public class Member(Snowflake user) : PermissionOverwrite(user, PermissionOverwriteType.Member);
    
    [method: SetsRequiredMembers]
    public class Role(Snowflake role) : PermissionOverwrite(role, PermissionOverwriteType.Role);
}
