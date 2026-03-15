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
using Fluxify.Application.State;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Text;
using Fluxify.Rest;

namespace Fluxify.Application.Repositories;

public class ChannelRepository(RestClient client, ChannelMapper mapper)
{
    public event Action<IChannel, ChangeType>? OnChange;
    private readonly PermanentCache<IChannel, ChannelMapper> _cache = new(mapper);

    public async Task<IChannel> GetAsync(Snowflake id, bool bypassCache = false)
    {
        var channel = await _cache.GetOrCreateAsync(id, GetChannelRestAsync, bypassCache);
        
        OnChange?.Invoke(channel, ChangeType.Update);
        
        return channel;
    }

    internal void Insert(ChannelResponse response)
        => _cache.UpdateOrCreate(mapper.FromDto(response, 
            response.ParentId is { } pId ? _cache.GetCachedOrDefault<GuildCategory>(pId) : null));

    private async Task<IChannel?> GetChannelRestAsync(Snowflake id) 
        => await client.Channels[id].GetAsync() is {} channel 
            ? await mapper.FromDtoAsync(channel) 
            : throw new Exception($"Couldn't get channel with id {id}");

    public async Task<IChannel?> CreateAsync(Snowflake guildId)
    {
        var channelAsync = await client.Guilds[guildId].CreateChannelAsync(new ChannelCreateTextRequest("", null, null, null, null));
        if (channelAsync is null) return null;

        var entity = await mapper.FromDtoAsync(channelAsync);
        OnChange?.Invoke(entity, ChangeType.Create);
        return _cache.UpdateOrCreate(entity);
    }

    public async Task DeleteAsync(Snowflake id, bool silent = false)
    {
        await client.Channels[id].DeleteAsync(silent);
        _cache.Remove(id);
    }
    
    internal void Reset() => _cache.Clear();
}