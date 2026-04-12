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
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Channels.Partial;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Model.Channel;
using Fluxify.Application.State;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Category;
using Fluxify.Dto.Channels.LinkChannel;
using Fluxify.Dto.Channels.Text;
using Fluxify.Dto.Channels.Voice;

namespace Fluxify.Application.Entities.Channels;

[Mapper]
internal partial class ChannelMapper(FluxerApplication application)
    : IUpdateEntity<IChannel, ChannelResponse>,
        ICreateEntity<IChannel, ChannelResponse>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FluxerApplication App() => application;

    [UseMapper] public CacheMapper CacheMapper { get; } = application.CacheMapper;

    [MapValue("fluxerApplication", Use = nameof(App)),
     MapDerivedType<DmChannelResponse, Dm>,
     MapDerivedType<GroupDmChannelResponse, GroupDm>,
     MapProperty(nameof(PrivateChannelResponse.Recipients), nameof(PrivateTextChannel.RecipientsRef))]
    public partial PrivateTextChannel FromPrivateDto(PrivateChannelResponse dto);

    [MapValue("fluxerApplication", Use = nameof(App)),
     MapDerivedType<GuildCategoryResponse, GuildCategory>,
     MapProperty(nameof(GuildChannelResponse.GuildId), nameof(GuildChannel<,>.GuildRef))]
    public partial IGuildChannel FromGuildChannelDto(GuildChannelResponse dto);

    [UserMapping(Default = true),
     IncludeMappingConfiguration(nameof(FromGuildChannelDto)),
     MapDerivedType<GuildTextChannelResponse, GuildTextChannel>,
     MapDerivedType<GuildLinkChannelResponse, GuildLinkChannel>,
     MapDerivedType<GuildVoiceChannelResponse, GuildVoiceChannel>,
     MapProperty(nameof(NestedChannelResponse.ParentId), nameof(GuildNestedChannel<,>.ParentRef))]
    public partial IGuildChannel FromNestedDto(GuildChannelResponse dto);

    [NamedMapping("MapFromChannelResponse"),
     MapDerivedType<GuildChannelResponse, IGuildChannel>,
     MapDerivedType<PrivateChannelResponse, PrivateTextChannel>]
    public partial IChannel MapFromResponse(ChannelResponse dto);

    [MapperRequiredMapping(RequiredMappingStrategy.Target),
     MapValue(nameof(GroupDmProperties.Icon), null)]
    private partial GroupDmProperties ToProperties(GroupDm request);

    [MapperRequiredMapping(RequiredMappingStrategy.Target),
     MapDerivedType<GroupDm, GroupDmProperties>,
     MapDerivedType<GuildCategory, CategoryProperties>,
     MapDerivedType<GuildLinkChannel, LinkChannelProperties>,
     MapDerivedType<GuildTextChannel, TextChannelProperties>,
     MapDerivedType<GuildVoiceChannel, VoiceChannelProperties>]
    public partial ChannelProperties ToProperties(IChannel request);

    [MapperRequiredMapping(RequiredMappingStrategy.Target),
     MapDerivedType<TextChannelProperties, ChannelCreateTextRequest>,
     MapDerivedType<LinkChannelProperties, ChannelCreateLinkRequest>,
     MapDerivedType<CategoryProperties, ChannelCreateCategoryRequest>,
     MapDerivedType<VoiceChannelProperties, ChannelCreateVoiceRequest>]
    public partial ChannelCreateRequest ToCreateRequest(ChannelProperties request);

    [MapDerivedType<VoiceChannelProperties, ChannelUpdateVoiceRequest>,
     MapDerivedType<TextChannelProperties, ChannelUpdateTextRequest>,
     MapDerivedType<LinkChannelProperties, ChannelUpdateLinkRequest>,
     MapDerivedType<CategoryProperties, ChannelUpdateCategoryRequest>]
    public partial ChannelUpdateRequest ToUpdateRequest(ChannelProperties request);

    [MapDerivedType<ChannelMemberPermissionOverwrite, PermissionOverwrite.Member>,
     MapDerivedType<ChannelRolePermissionOverwrite, PermissionOverwrite.Role>]
    public partial PermissionOverwrite FromDto(ChannelPermissionOverwrite dto);

    [MapDerivedType<PermissionOverwrite.Member, ChannelMemberPermissionOverwrite>,
     MapDerivedType<PermissionOverwrite.Role, ChannelRolePermissionOverwrite>]
    public partial ChannelPermissionOverwrite ToDto(PermissionOverwrite model);

    [IncludeMappingConfiguration(nameof(FromGuildChannelDto)),
     MapperIgnoreSource(nameof(PrivateChannelResponse.Id))]
    public partial void UpdateGuildEntity([MappingTarget] IGuildChannel data, GuildChannelResponse update);

    [UserMapping(Default = true),
     IncludeMappingConfiguration(nameof(FromGuildChannelDto)),
     MapperIgnoreSource(nameof(PrivateChannelResponse.Id)),
     MapDerivedType<GuildTextChannelResponse, GuildTextChannel>,
     MapDerivedType<GuildLinkChannelResponse, GuildLinkChannel>,
     MapDerivedType<GuildVoiceChannelResponse, GuildVoiceChannel>,
     MapProperty(nameof(NestedChannelResponse.ParentId), nameof(GuildNestedChannel<,>.ParentRef))]
    public partial void UpdateNestedEntity([MappingTarget] IGuildChannel data, GuildChannelResponse update);

    [IncludeMappingConfiguration(nameof(FromPrivateDto)),
     MapperIgnoreSource(nameof(PrivateChannelResponse.Id))]
    public partial void UpdatePrivateEntity([MappingTarget] PrivateTextChannel data, PrivateChannelResponse update);

    public void UpdateEntity([MappingTarget] IChannel data, ChannelResponse update)
    {
        switch (data)
        {
            case PrivateTextChannel privateChannel:
                UpdatePrivateEntity(privateChannel, (PrivateChannelResponse)update);
                break;
            case IGuildChannel guildChannel:
                UpdateNestedEntity(guildChannel, (GuildChannelResponse)update);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public partial PartialChannel FromPartialResponse(ChannelPartialResponse response);
}