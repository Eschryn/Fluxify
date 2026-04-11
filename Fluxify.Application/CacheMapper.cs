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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Guilds.Invite;
using Fluxify.Dto.Invites;
using Fluxify.Dto.Users;

namespace Fluxify.Application;

internal partial class CacheMapper(FluxerApplication app)
{
    [return: NotNullIfNotNull(nameof(channelId))]
    public CacheRef<IChannel>? ResolveChannel(Snowflake? channelId)
        => channelId is { } id ? app.ChannelsRepository.GetCachedOrDefault(id) : null;
    
    [return: NotNullIfNotNull(nameof(channelId))]
    public ICacheRef<TChannel>? ResolveChannelTyped<TChannel>(Snowflake? channelId) where TChannel : class, IChannel
        => channelId is { } id ? app.ChannelsRepository.GetCachedOrDefault(id).Cast<TChannel>() : null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(channelId))]
    public ICacheRef<GuildTextChannel>? ResolveChannelGuildText(Snowflake? channelId) 
        => ResolveChannelTyped<GuildTextChannel>(channelId);
    
    [return: NotNullIfNotNull(nameof(guildId))]
    public CacheRef<Guild>? ResolveGuild(Snowflake? guildId)
        => guildId is { } id ? app.GuildsRepository.Cache.GetCachedOrDefault(id) : null;

    [return: NotNullIfNotNull(nameof(userId))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ICacheRef<IUser>? ResolveUser(Snowflake? userId) => ResolveGlobalUser(userId);

    [return: NotNullIfNotNull(nameof(userId))]
    public CacheRef<GlobalUser>? ResolveGlobalUser(Snowflake? userId)
        => userId is { } id ? ResolveGlobalUser(id) : null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CacheRef<GlobalUser> ResolveGlobalUser(Snowflake userId)
        => app.UsersRepository.GetCachedOrDefault(userId);
    
    [return: NotNullIfNotNull(nameof(user))]
    public CacheRef<GlobalUser>? InsertGlobalUser(UserPartialResponse? user)
        => user is { } ? app.UsersRepository.Insert(user) : null;
    
    [return: NotNullIfNotNull(nameof(user))]
    public ICacheRef<IUser>? InsertGlobalUserAsIUser(UserPartialResponse? user)
        => user is { } ? app.UsersRepository.Insert(user) : null;
    
    public CacheRef<IChannel> GetChannel(ChannelPartialResponse response)
        => app.ChannelsRepository.GetCachedOrDefault(response.Id) is
            { Value: not null } cacheRef
            ? cacheRef
            : new CacheRef<IChannel>(response.Id, app.ChannelMapper.FromPartialResponse(response));
    
    public ICacheRef<IUser> ResolveCachedInviter(InviteResponseSchema invite)
    {
        var user = app.UsersRepository.Insert(invite.Inviter);
        if (invite is not GuildInviteResponse or GuildInviteMetadataResponse)
        {
            return user;
        }

        var channel = invite switch
        {
            GuildInviteResponse response => response.Channel,
            GuildInviteMetadataResponse response => response.Channel,
            _ => throw new ArgumentOutOfRangeException(nameof(invite), invite, null)
        };
        
        if (app.ChannelsRepository.GetCachedOrDefault(channel.Id).Value is IGuildChannel { Guild.MembersRepository: {} membersRepository })
        {
            return membersRepository.Cache.GetCachedOrDefault(user.Id);
        }
        
        return user;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Snowflake? CacheRefToId(CacheRef<IChannel>? @ref) => @ref?.Id;
}