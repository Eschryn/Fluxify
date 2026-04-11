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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Image? CreateProfileImage(UserPartialResponse dto)
        => application.ImageFactory.MakeAvatar(dto.Id, dto.Avatar, null, null);

    [MapDerivedType<UserPartialResponse, GlobalUser>,
     MapDerivedType<UserPrivateReponse, PrivateUser>]
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

    [MapperIgnoreSource(nameof(VoiceStateResponse.GuildId)),
     MapperIgnoreSource(nameof(VoiceStateResponse.ChannelId)),
     MapperIgnoreSource(nameof(VoiceStateResponse.UserId)),
     MapperIgnoreSource(nameof(VoiceStateResponse.Member))]
    public partial void UpdateVoiceState([MappingTarget] VoiceState target, VoiceStateResponse state);

    [MapDerivedType<PresenceResponse, UserPresence>,
     MapperIgnoreSource(nameof(PresenceResponse.User)),
     MapPropertyFromSource(nameof(@IPresence.CustomStatus.Emoji), Use = nameof(MapStatusEmoji))]
    public partial void UpdateStatus([MappingTarget] IPresence target, PresenceResponse source);

    private IEmoji? MapStatusEmoji(PresenceResponse presence)
    {
        return presence.CustomStatus switch
        {
            { EmojiId: { } id, EmojiName: var name, EmojiAnimated: var animated } => new GuildEmoji
            {
                Id = id,
                Name = name ?? string.Empty,
                IsAnimated = animated ?? false
            },
            { EmojiId: null, EmojiName: { } name } => new UnicodeEmoji(name),
            _ => null
        };
    }
}