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
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Users;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Users;

[Mapper]
[UseStaticMapper(typeof(CommonMapper))]
public partial class UserMapper(FluxerApplication application) : IUpdateEntity<GlobalUser>, IUpdateEntity<IGuildMember>
{
    [MapProperty(nameof(UserPartialResponse.Avatar), nameof(GlobalUser.AvatarHash))]
    [MapValue(nameof(GlobalUser.Status), UserStatus.Offline)]
    [MapValue(nameof(GlobalUser.IsAfk), false)]
    [MapValue(nameof(GlobalUser.IsMobile), false)]
    [MapValue(nameof(GlobalUser.CustomStatus), null)]
    [MapValue(nameof(GlobalUser.BannerHash), null)]
    private partial GlobalUser Map(UserPartialResponse dto, FluxerApplication fluxerApplication);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GlobalUser Map(UserPartialResponse dto) => Map(dto, application);
    
    [MapValue(nameof(GlobalUser.Status), UserStatus.Offline)]
    [MapValue(nameof(GlobalUser.IsAfk), false)]
    [MapValue(nameof(GlobalUser.IsMobile), false)]
    [MapValue(nameof(GlobalUser.CustomStatus), null)]
    private partial PrivateUser Map(UserPrivateReponse dto, FluxerApplication fluxerApplication);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PrivateUser Map(UserPrivateReponse dto) => Map(dto, application);
    
    [MapProperty(nameof(UserPartialResponse.Avatar), nameof(GlobalUser.AvatarHash))]
    [MapValue(nameof(GlobalUser.Status), UserStatus.Offline)]
    [MapValue(nameof(GlobalUser.IsAfk), false)]
    [MapValue(nameof(GlobalUser.IsMobile), false)]
    [MapValue(nameof(GlobalUser.CustomStatus), null)]
    [MapValue(nameof(GlobalUser.BannerHash), null)]
    private partial WebhookUser MapWebhook(UserPartialResponse dto, FluxerApplication fluxerApplication);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public WebhookUser MapWebhook(UserPartialResponse dto) => MapWebhook(dto, application);
    
    [MapProperty(nameof(GuildMemberResponse.Roles), nameof(GuildMember.AssignedRoleIds))]
    [MapperIgnoreTarget(nameof(GuildMember.Roles))]
    [MapperIgnoreTarget(nameof(GuildMember.IsAfk))]
    [MapperIgnoreTarget(nameof(GuildMember.IsMobile))]
    [MapperIgnoreTarget(nameof(GuildMember.Status))]
    [MapperIgnoreTarget(nameof(GuildMember.CustomStatus))]
    [MapperIgnoreSource(nameof(GuildMemberResponse.User))]
    private partial GuildMember Map(GuildMemberResponse dto, GlobalUser user, Snowflake id, Guild guild, FluxerApplication fluxerApplication);
    public GuildMember Map(GuildMemberResponse dto, GlobalUser user, Guild guild) => Map(dto, user, user.Id, guild, application);

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
    [MapperIgnoreSource(nameof(IGuildMember.Guild))]
    [MapperIgnoreTarget(nameof(GuildMember.User))]
    [MapperIgnoreSource(nameof(GuildMember.User))]
    public partial void UpdateEntity([MappingTarget] GuildMember data, GuildMember update);
    
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
    [MapperIgnoreTarget(nameof(GlobalUser.BannerHash))]
    [MapperIgnoreSource(nameof(PresenceResponse.User))]
    [MapProperty(nameof(PresenceResponse.Status), nameof(GlobalUser.Status))]
    [MapProperty(nameof(PresenceResponse.Mobile), nameof(GlobalUser.IsMobile))]
    [MapProperty(nameof(PresenceResponse.Afk), nameof(GlobalUser.IsAfk))]
    [MapProperty(nameof(PresenceResponse.CustomStatus), nameof(GlobalUser.CustomStatus))]
    public partial void UpdateStatus([MappingTarget] GlobalUser target, PresenceResponse source);

    public void UpdateEntity(IGuildMember data, IGuildMember update) => UpdateEntity((GuildMember)data, (GuildMember)update);
}