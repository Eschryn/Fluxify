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
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Model.Guild;
using Fluxify.Application.State;
using Fluxify.Dto.Users;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;

namespace Fluxify.Application.Entities.Users;

[Mapper]
[UseStaticMapper(typeof(CommonMapper))]
public partial class UserMapper(FluxerApplication application)
    : IUpdateEntity<GlobalUser, UserPartialResponse>,
        ICreateEntity<GlobalUser, UserPartialResponse>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private FluxerApplication GetApplication() => application;
    
    [UseMapper] private CacheMapper CacheMapper => application.CacheMapper;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Image? CreateProfileImage(UserPartialResponse dto)
        => application.ImageFactory.MakeAvatar(dto.Id, dto.Avatar, null, null);

    [MapDerivedType<UserPrivateReponse, PrivateUser>,
     MapDerivedType<UserPartialResponse, GlobalUser>]
    public partial GlobalUser MapFromResponse(UserPartialResponse dto);

    [MapperIgnoreTarget(nameof(GlobalUser.Presence)),
     MapValue("fluxerApplication", Use = nameof(GetApplication)),
     MapPropertyFromSource(nameof(GlobalUser.Avatar), Use = nameof(CreateProfileImage))]
    public partial GlobalUser MapUserResponse(UserPartialResponse dto);

    [IncludeMappingConfiguration(nameof(MapUserResponse)),
     MapperIgnoreSource(nameof(UserPartialResponse.Id))]
    public partial void UpdateEntity([MappingTarget] GlobalUser data, UserPartialResponse update);

    [IncludeMappingConfiguration(nameof(MapUserResponse))]
    public partial PrivateUser MapUserPrivate(UserPrivateReponse dto);

    [IncludeMappingConfiguration(nameof(MapUserResponse))]
    public partial WebhookUser MapWebhook(UserPartialResponse dto);

    [MapperIgnoreSource(nameof(VoiceStateResponse.UserId)),
     MapperIgnoreSource(nameof(VoiceStateResponse.Member)),
     MapProperty(nameof(VoiceStateResponse.GuildId), nameof(VoiceState.GuildRef)),
     MapProperty(nameof(VoiceStateResponse.ChannelId), nameof(VoiceState.VoiceChannelRef))]
    public partial VoiceState MakeVoiceState(VoiceStateResponse state);
    
    [MapperIgnoreSource(nameof(VoiceStateResponse.UserId)),
     MapperIgnoreSource(nameof(VoiceStateResponse.Member)),
     MapProperty(nameof(VoiceStateResponse.GuildId), nameof(VoiceState.GuildRef)),
     MapProperty(nameof(VoiceStateResponse.ChannelId), nameof(VoiceState.VoiceChannelRef))]
    public partial void UpdateVoiceState([MappingTarget] VoiceState target, VoiceStateResponse state);

    [MapDerivedType<PresenceResponse, UserPresence>,
     MapperIgnoreSource(nameof(PresenceResponse.User)),
     MapProperty(nameof(PresenceResponse.CustomStatus), nameof(IPresence.CustomStatus), Use = nameof(MapCustomStatus))]
    public partial void UpdateStatus([MappingTarget] IPresence target, PresenceResponse source);

    [MapPropertyFromSource(nameof(CustomStatus.Emoji), Use = nameof(MapStatusEmoji))]
    private partial void MapCustomStatus([MappingTarget] CustomStatus? status, Fluxify.Dto.Users.CustomStatus? customStatus);

    private IEmoji MapStatusEmoji(Fluxify.Dto.Users.CustomStatus presence)
    {
        return presence switch
        {
            { EmojiId: { } id, EmojiName: var name, EmojiAnimated: var animated } => new GuildEmoji
            {
                Id = id,
                Name = name ?? string.Empty,
                IsAnimated = animated ?? false
            },
            { EmojiId: null, EmojiName: { } name } => new UnicodeEmoji(name),
            _ => throw new InvalidOperationException("Invalid presence status")
        };
    }
}