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

using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.State.Ref;
using Fluxify.Dto.Guilds.Roles;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Roles;

[Mapper]
[UseStaticMapper(typeof(CommonMapper))]
public partial class RoleMapper : IUpdateEntity<IRole>
{
    [MapProperty(nameof(GuildRoleResponse.Mentionable), nameof(Role.IsMentionable))]
    public partial Role MapFromDto(GuildRoleResponse dto, CacheRef<Guild> guildRef);
    
    public void UpdateEntity(IRole data, IRole update) => UpdateEntity((Role)data, (Role)update);
    [MapperIgnoreSource(nameof(Role.Guild))]
    [MapperIgnoreSource(nameof(Role.GuildRef))]
    public partial void UpdateEntity([MappingTarget] Role data, Role update);
}