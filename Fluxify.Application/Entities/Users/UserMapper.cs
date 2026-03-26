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
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Users;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Users;

[Mapper]
[UseStaticMapper(typeof(CommonMapper))]
public partial class UserMapper : IUpdateEntity<GlobalUser>, IUpdateEntity<GuildUser>
{
    [MapProperty(nameof(UserPartialResponse.Avatar), nameof(GlobalUser.AvatarHash))]
    [MapValue(nameof(GlobalUser.Status), UserStatus.Offline)]
    [MapValue(nameof(GlobalUser.IsAfk), false)]
    [MapValue(nameof(GlobalUser.IsMobile), false)]
    [MapValue(nameof(GlobalUser.CustomStatus), null)]
    public partial GlobalUser Map(UserPartialResponse dto);
    
    [MapValue(nameof(GlobalUser.Status), UserStatus.Offline)]
    [MapValue(nameof(GlobalUser.IsAfk), false)]
    [MapValue(nameof(GlobalUser.IsMobile), false)]
    [MapValue(nameof(GlobalUser.CustomStatus), null)]
    public partial PrivateUser Map(UserPrivateReponse dto);
    
    [MapProperty(nameof(UserPartialResponse.Avatar), nameof(GlobalUser.AvatarHash))]
    [MapValue(nameof(GlobalUser.Status), UserStatus.Offline)]
    [MapValue(nameof(GlobalUser.IsAfk), false)]
    [MapValue(nameof(GlobalUser.IsMobile), false)]
    [MapValue(nameof(GlobalUser.CustomStatus), null)]
    public partial WebhookUser MapWebhook(UserPartialResponse dto);
    
    [MapProperty(nameof(GuildMemberResponse.Roles), nameof(GuildUser.AssignedRoleIds))]
    [MapperIgnoreTarget(nameof(GuildUser.Roles))]
    [MapperIgnoreTarget(nameof(GuildUser.IsAfk))]
    [MapperIgnoreTarget(nameof(GuildUser.IsMobile))]
    [MapperIgnoreTarget(nameof(GuildUser.Status))]
    [MapperIgnoreTarget(nameof(GuildUser.CustomStatus))]
    [MapperIgnoreTarget(nameof(GuildUser.VoiceState))]
    [MapperIgnoreSource(nameof(GuildMemberResponse.User))]
    private partial GuildUser Map(GuildMemberResponse dto, GlobalUser user, Snowflake id, Guild guild);
    public GuildUser Map(GuildMemberResponse dto, GlobalUser user, Guild guild) => Map(dto, user, user.Id, guild);

    [MapperIgnoreTarget(nameof(GlobalUser.Id))]
    [MapperIgnoreSource(nameof(GlobalUser.Id))]
    [MapperIgnoreSource(nameof(GlobalUser.Status))]
    [MapperIgnoreSource(nameof(GlobalUser.CustomStatus))]
    [MapperIgnoreSource(nameof(GlobalUser.IsAfk))]
    [MapperIgnoreSource(nameof(GlobalUser.IsMobile))]
    [MapperIgnoreTarget(nameof(GlobalUser.Status))]
    [MapperIgnoreTarget(nameof(GlobalUser.CustomStatus))]
    [MapperIgnoreTarget(nameof(GlobalUser.IsAfk))]
    [MapperIgnoreTarget(nameof(GlobalUser.IsMobile))]
    public partial void UpdateEntity([MappingTarget] GlobalUser data, GlobalUser update);

    [MapperIgnoreTarget(nameof(GlobalUser.Id))]
    [MapperIgnoreSource(nameof(GlobalUser.Id))]
    [MapperIgnoreSource(nameof(GlobalUser.Status))]
    [MapperIgnoreSource(nameof(GlobalUser.CustomStatus))]
    [MapperIgnoreSource(nameof(GlobalUser.IsAfk))]
    [MapperIgnoreSource(nameof(GlobalUser.IsMobile))]
    [MapperIgnoreTarget(nameof(GlobalUser.Status))]
    [MapperIgnoreTarget(nameof(GlobalUser.CustomStatus))]
    [MapperIgnoreTarget(nameof(GlobalUser.IsAfk))]
    [MapperIgnoreTarget(nameof(GlobalUser.IsMobile))]
    [MapperIgnoreSource(nameof(GuildUser.VoiceState))]
    [MapperIgnoreTarget(nameof(GuildUser.VoiceState))]
    public partial void UpdateEntity([MappingTarget] GuildUser data, GuildUser update);
    
    [MapperIgnoreSource(nameof(VoiceStateResponse.GuildId))]
    [MapperIgnoreSource(nameof(VoiceStateResponse.ChannelId))]
    [MapperIgnoreSource(nameof(VoiceStateResponse.UserId))]
    [MapperIgnoreSource(nameof(VoiceStateResponse.Member))]
    public partial void UpdateVoiceState([MappingTarget] VoiceState target, VoiceStateResponse state);
    
    [MapperIgnoreTarget(nameof(GlobalUser.Id))]
    [MapperIgnoreTarget(nameof(GlobalUser.Bot))]
    [MapperIgnoreTarget(nameof(GlobalUser.System))]
    [MapperIgnoreTarget(nameof(GlobalUser.AvatarColor))]
    [MapperIgnoreTarget(nameof(GlobalUser.AvatarHash))]
    [MapperIgnoreTarget(nameof(GlobalUser.Discriminator))]
    [MapperIgnoreTarget(nameof(GlobalUser.Username))]
    [MapperIgnoreTarget(nameof(GlobalUser.GlobalName))]
    [MapperIgnoreTarget(nameof(GlobalUser.Flags))]
    [MapperIgnoreSource(nameof(PresenceResponse.UserPartial))]
    [MapProperty(nameof(PresenceResponse.Status), nameof(GlobalUser.Status))]
    [MapProperty(nameof(PresenceResponse.Mobile), nameof(GlobalUser.IsMobile))]
    [MapProperty(nameof(PresenceResponse.Afk), nameof(GlobalUser.IsAfk))]
    [MapProperty(nameof(PresenceResponse.CustomStatus), nameof(GlobalUser.CustomStatus))]
    public partial void UpdateStatus([MappingTarget] GlobalUser target, PresenceResponse source);
}