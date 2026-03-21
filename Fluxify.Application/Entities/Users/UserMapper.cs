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
using Fluxify.Application.Repositories;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Users;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Users;

[Mapper]
[UseStaticMapper(typeof(CommonMapper))]
public partial class UserMapper : IUpdateEntity<GlobalUser>, IUpdateEntity<GuildUser>
{
    [MapProperty(nameof(UserPartialResponse.Avatar), nameof(GlobalUser.AvatarHash))]
    public partial GlobalUser Map(UserPartialResponse dto);
    
    public partial PrivateUser Map(UserPrivateReponse dto);
    
    [MapProperty(nameof(UserPartialResponse.Avatar), nameof(GlobalUser.AvatarHash))]
    public partial WebhookUser MapWebhook(UserPartialResponse dto);
    
    [MapProperty(nameof(GuildMemberResponse.Roles), nameof(GuildUser.AssignedRoleIds))]
    [MapperIgnoreTarget(nameof(GuildUser.Roles))]
    [MapperIgnoreSource(nameof(GuildMemberResponse.User))]
    private partial GuildUser Map(GuildMemberResponse dto, IUser user, Snowflake id, Guild guild);
    public GuildUser Map(GuildMemberResponse dto, IUser user, Guild guild) => Map(dto, user, user.Id, guild);

    [MapperIgnoreTarget(nameof(GlobalUser.Id))]
    [MapperIgnoreSource(nameof(GlobalUser.Id))]
    public partial void UpdateEntity([MappingTarget] GlobalUser data, GlobalUser update);

    [MapperIgnoreTarget(nameof(GlobalUser.Id))]
    [MapperIgnoreSource(nameof(GlobalUser.Id))]
    public partial void UpdateEntity([MappingTarget] GuildUser data, GuildUser update);
}