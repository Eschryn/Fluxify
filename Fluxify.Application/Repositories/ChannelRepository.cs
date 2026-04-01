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
using System.Diagnostics.CodeAnalysis;
using Fluxify.Application.Common;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Model.Channel;
using Fluxify.Application.State;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Users;
using Fluxify.Rest;

namespace Fluxify.Application.Repositories;

public sealed class ChannelRepository(RestClient client, ChannelMapper mapper, CacheConfig config)
{
    internal event Action<IChannel, ChangeType>? OnChange;
    internal readonly ICache<IChannel> Cache = ICache<IChannel>.CreateLru(config.ChannelCacheSize, mapper);
    private readonly ConcurrentDictionary<Snowflake, PrivateTextChannel> _privateChannels = new();

    public async Task<IChannel> GetAsync(Snowflake guildId, bool bypassCache = false)
    {
        var channel = await Cache.GetOrCreateAsync(guildId, GetChannelRestAsync, bypassCache);

        OnChange?.Invoke(channel, ChangeType.Update);
        
        return channel;
    }

    internal async Task<TChannel> CreateOrGetPrivateChannelAsync<TChannel>(CreatePrivateChannelRequest request, CancellationToken cancellationToken)
        where TChannel : PrivateTextChannel
    {
        if (request.RecipientId.HasValue && _privateChannels.TryGetValue(request.RecipientId.Value, out var channel))
        {
            return (TChannel)channel;
        }
        
        var result = await client.Users.Me.PrivateChannels.CreateAsync(request, cancellationToken);
        return (TChannel)Insert(result!);
    }
    
    internal IChannel Insert(ChannelResponse response)
    {
        var updateOrCreate = Cache.UpdateOrCreate(mapper.FromDto(response));
        if (updateOrCreate is IGuildChannel guildChannel)
        {
            guildChannel.Guild.GuildChannels.AddOrUpdate(
                guildChannel.Id,
                guildChannel,
                (key, value) =>
                {
                    mapper.UpdateEntity(value, guildChannel);
                    return value;
                });
        }

        if (updateOrCreate is Dm dmChannel)
        {
            var snowflake = dmChannel.Recipients!.Single().Id;
            _privateChannels.TryAdd(snowflake, dmChannel);
        }

        return updateOrCreate;
    }

    internal T? GetCachedOrDefault<T>(Snowflake id) where T : IChannel => Cache.GetCachedOrDefault<T>(id);

    private async Task<IChannel> GetChannelRestAsync(Snowflake id)
        => await client.Channels[id].GetAsync() is { } channel
            ? await mapper.FromDtoAsync(channel)
            : throw new Exception($"Couldn't get channel with id {id}");

    internal async Task<TChannel> CreateAsync<TChannel>(Snowflake guildId, ChannelProperties textChannelRequest)
        where TChannel : IGuildChannel
    {
        var channelCreateRequest = mapper.ToCreateRequest(textChannelRequest);
        var channelAsync = await client.Guilds[guildId].CreateChannelAsync(channelCreateRequest);

        var entity = Insert(channelAsync ?? throw new Exception("Channel was not created"));
        OnChange?.Invoke(entity, ChangeType.Create);
        return (TChannel)entity;
    }

    internal async Task UpdateAsync<TChannel, TProperties>(
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
        
        Insert(response);
    }

    public async Task DeleteAsync(Snowflake id, bool silent = false)
    {
        await client.Channels[id].DeleteAsync(silent);
        Remove(id, out _);
    }

    internal bool Remove(Snowflake id, [NotNullWhen(true)] out IChannel? removedChannel)
    {
        if (Cache.GetCachedOrDefault<IGuildChannel>(id) is { Guild: { } guild })
        {
            guild.GuildChannels.Remove(id, out _);
        }

        if (Cache.Remove(id, out removedChannel) && removedChannel is Dm { Recipients: {} recipients })
        {
            var recipient = recipients.Single();
            _privateChannels.TryRemove(recipient.Id, out _);
        }
        
        return removedChannel != null;
    }

    internal void Reset() => Cache.Clear();
}