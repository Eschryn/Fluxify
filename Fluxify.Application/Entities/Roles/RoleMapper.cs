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

using System.Runtime.CompilerServices;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.State;
using Fluxify.Dto.Guilds.Roles;

namespace Fluxify.Application.Entities.Roles;

internal readonly record struct RoleInsert(GuildRoleResponse Response, CacheRef<Guild> GuildRef);

[Mapper]
[UseStaticMapper(typeof(CommonMapper))]
internal partial class RoleMapper 
    : IUpdateEntity<IRole, RoleInsert>,
        ICreateEntity<IRole, RoleInsert>
{
    [MapNestedProperties(nameof(RoleInsert.Response))]
    private partial Role MapRoleFromResponse(RoleInsert data);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IRole MapFromResponse(RoleInsert data) => MapRoleFromResponse(data);

    [MapNestedProperties(nameof(RoleInsert.Response)),
     MapperIgnoreSource(nameof(RoleInsert.GuildRef)),
     MapDerivedType<RoleInsert, Role>]
    public partial void UpdateEntity([MappingTarget] IRole data, RoleInsert update);
}