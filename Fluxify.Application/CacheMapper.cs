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
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Application;

internal partial class CacheMapper(FluxerApplication app)
{
    [return: NotNullIfNotNull(nameof(channelId))]
    public CacheRef<IChannel>? ResolveChannel(Snowflake? channelId)
        => channelId is { } id ? app.ChannelsRepository.GetCachedOrDefault(id) : null;
    
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
    
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Snowflake? CacheRefToId(CacheRef<IChannel>? @ref) => @ref?.Id;
}