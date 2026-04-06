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

using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.State.Ref;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Guilds.Invite;
using Fluxify.Dto.Invites;
using Fluxify.Dto.Users;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Invites;

[Mapper]
public partial class InviteMapper(FluxerApplication fluxerApplication)
{
    private ICacheRef<IUser> GetUser(UserPartialResponse response, IChannel channel)
        => (channel as IGuildChannel)?.Guild?.MembersRepository.Cache
            .GetCachedOrDefault(response.Id) is { Value: not null } memberRef
                ? memberRef
                : fluxerApplication.UsersRepository.Insert(response);

    private CacheRef<IChannel> GetChannel(ChannelPartialResponse response)
        => fluxerApplication.ChannelsRepository.GetCachedOrDefault(response.Id) is
            { Value: not null } cacheRef
            ? cacheRef
            : new CacheRef<IChannel>(response.Id, fluxerApplication.ChannelMapper.FromPartialResponse(response));

    [MapDerivedType<GuildInviteMetadataResponse, GuildChannelInviteMetadata>]
    public partial IInviteMetadata MapFromResponse(InviteMetadataResponseSchema response);

    [MapperIgnoreSource(nameof(GuildInviteMetadataResponse.Guild))]
    [MapperIgnoreSource(nameof(GuildInviteMetadataResponse.Channel))]
    [MapperIgnoreSource(nameof(GuildInviteMetadataResponse.Inviter))]
    private partial GuildChannelInviteMetadata MapFromResponse(
        GuildInviteMetadataResponse response,
        ICacheRef<IUser>? inviter,
        CacheRef<IChannel> channel
    );

    public GuildChannelInviteMetadata MapFromResponse(GuildInviteMetadataResponse response)
    {
        var channel = GetChannel(response.Channel);
        var inviter = response.Inviter is { } user ? GetUser(user, channel.Value!) : null;
        return MapFromResponse(response, inviter, channel);
    }

    [MapperIgnoreSource(nameof(GuildInviteResponse.Guild))]
    [MapperIgnoreSource(nameof(GuildInviteResponse.Channel))]
    [MapperIgnoreSource(nameof(GuildInviteResponse.Inviter))]
    private partial GuildChannelInvite MapFromResponse(
        GuildInviteResponse response,
        ICacheRef<IUser>? inviter,
        CacheRef<IChannel> channel
    );

    public GuildChannelInvite MapFromResponse(GuildInviteResponse response)
    {
        var channel = GetChannel(response.Channel);
        var inviter = response.Inviter is { } user ? GetUser(user, channel.Value) : null;
        return MapFromResponse(response, inviter, channel);
    }
}