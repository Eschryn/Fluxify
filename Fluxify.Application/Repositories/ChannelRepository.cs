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

using System.Collections.Concurrent;
using Fluxify.Application.Common;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Model.Channel;
using Fluxify.Application.State;
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Users;
using Fluxify.Rest;

namespace Fluxify.Application.Repositories;

internal sealed class ChannelRepository(RestClient client, ChannelMapper mapper, CacheConfig config)
{
    internal readonly ICache<IChannel> Cache = ICache<IChannel>.CreateLru(config.ChannelCacheSize, mapper);
    private readonly ConcurrentDictionary<Snowflake, ICacheRef<PrivateTextChannel>> _privateChannels = new();

    public async Task<CacheRef<IChannel>> GetAsync(Snowflake guildId, bool bypassCache = false)
    {
        return await Cache.GetOrCreateAsync(guildId, GetChannelRestAsync, bypassCache);
    }

    internal async Task<TChannel> CreateOrGetPrivateChannelAsync<TChannel>(CreatePrivateChannelRequest request, CancellationToken cancellationToken)
        where TChannel : PrivateTextChannel
    {
        if (request.RecipientId.HasValue && _privateChannels.TryGetValue(request.RecipientId.Value, out var channel))
        {
            return (TChannel)channel.Value!;
        }
        
        var result = await client.Users.Me.PrivateChannels.CreateAsync(request, cancellationToken);
        return (TChannel)Insert(result!).Value!;
    }
    
    internal CacheRef<IChannel> Insert(ChannelResponse response, CacheRef<Guild>? guildRef = null)
    {
        var updateOrCreate = Cache.UpdateOrCreate(mapper.FromDto(response, guildRef));
        if (updateOrCreate.Value is IGuildChannel { Guild: {} guild } guildChannel)
        {
            guild.GuildChannels.UpdateOrCreate(guildChannel);
        }

        if (updateOrCreate.Value is Dm dmChannel)
        {
            var snowflake = dmChannel.Recipients!.Single().Id;
            _privateChannels.TryAdd(snowflake, updateOrCreate.Cast<PrivateTextChannel>());
        }

        return updateOrCreate;
    }

    internal CacheRef<IChannel> GetCachedOrDefault(Snowflake id) => Cache.GetCachedOrDefault(id);

    private async Task<IChannel> GetChannelRestAsync(Snowflake id)
        => await client.Channels[id].GetAsync() is { } channel
            ? mapper.FromDto(channel)
            : throw new Exception($"Couldn't get channel with id {id}");

    internal async Task<TChannel> CreateAsync<TChannel>(Snowflake guildId, ChannelProperties textChannelRequest)
        where TChannel : class, IGuildChannel
    {
        var channelCreateRequest = mapper.ToCreateRequest(textChannelRequest);
        var channelAsync = await client.Guilds[guildId].CreateChannelAsync(channelCreateRequest);

        var entity = Insert(channelAsync ?? throw new Exception("Channel was not created"));
        return (TChannel)entity.Value!;
    }

    internal async Task<TChannel> UpdateAsync<TChannel, TProperties>(
        TChannel channel,
        Action<TProperties> configure,
        string? reason = null,
        CancellationToken cancellationToken = default
    )
        where TProperties : ChannelProperties
        where TChannel : IChannel
    {
        var properties = mapper
                .ToProperties(channel)
                .Cast<TProperties>()
                .Configure(configure);
        
        var request = mapper.ToUpdateRequest(properties);
        var response = await client.Channels[channel.Id].UpdateAsync(
            request,
            reason,
            cancellationToken
        ) ?? throw new Exception("Channel was not updated");
        
        return (TChannel)Insert(response).Value!;
    }

    public async Task DeleteAsync(Snowflake id, bool silent = false)
    {
        await client.Channels[id].DeleteAsync(silent);
        Remove(id, out _);
    }

    internal bool Remove(Snowflake id, out CacheRef<IChannel> removedChannel)
    {
        if (Cache.GetCachedOrDefault(id).Value is IGuildChannel { Guild: { } guild })
        {
            guild.GuildChannels.Remove(id, out _);
        }

        if (Cache.Remove(id, out removedChannel) && removedChannel.Value is Dm { Recipients: {} recipients })
        {
            var recipient = recipients.Single();
            _privateChannels.TryRemove(recipient.Id, out _);
        }
        
        return removedChannel != null;
    }

    internal void Reset() => Cache.Clear();
}