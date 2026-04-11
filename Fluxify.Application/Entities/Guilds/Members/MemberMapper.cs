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
using Fluxify.Application.Common;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.State;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Application.Entities.Guilds.Members;

internal readonly record struct MemberInsert(
    GuildMemberResponse Response,
    CacheRef<GlobalUser> UserRef,
    CacheRef<Guild> GuildRef
);

[Mapper]
[UseStaticMapper(typeof(CommonMapper))]
internal partial class MemberMapper(FluxerApplication application)
    : IUpdateEntity<IGuildMember, MemberInsert>,
        ICreateEntity<IGuildMember, MemberInsert>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private FluxerApplication GetApplication() => application;
    
    [MapValue("fluxerApplication", Use = nameof(GetApplication)),
     MapNestedProperties(nameof(MemberInsert.Response)),
     MapPropertyFromSource(nameof(GuildMember.Avatar), Use = nameof(CreateMemberProfileImage)),
     MapPropertyFromSource(nameof(GuildMember.Banner), Use = nameof(CreateMemberBannerImage)),
     MapProperty(nameof(@MemberInsert.Response.Roles), nameof(GuildMember.AssignedRoleIds)),
     MapperIgnoreTarget(nameof(GuildMember.ImmutableUser)),
     MapDerivedType<MemberInsert, GuildMember>]
    public partial IGuildMember MapFromResponse(MemberInsert insert);

    [MapNestedProperties(nameof(MemberInsert.Response)),
     MapperIgnoreSource(nameof(MemberInsert.UserRef)),
     MapperIgnoreSource(nameof(MemberInsert.GuildRef)),
     MapPropertyFromSource(nameof(GuildMember.Avatar), Use = nameof(CreateMemberProfileImage)),
     MapPropertyFromSource(nameof(GuildMember.Banner), Use = nameof(CreateMemberBannerImage)),
     MapProperty(nameof(@MemberInsert.Response.Roles), nameof(GuildMember.AssignedRoleIds)),
     MapValue(nameof(GuildMember.ImmutableUser), null),
     MapDerivedType<MemberInsert, GuildMember>]
    public partial void UpdateEntity([MappingTarget] IGuildMember data, MemberInsert update);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Image? CreateMemberProfileImage(MemberInsert insert)
        => application.ImageFactory.MakeAvatar(insert.UserRef.Id, insert.Response.Avatar, null, null);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Image? CreateMemberBannerImage(MemberInsert insert)
        => application.ImageFactory.MakeAvatar(insert.UserRef.Id, insert.Response.Banner, null, null);
}