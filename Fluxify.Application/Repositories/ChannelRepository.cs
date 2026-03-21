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

public sealed class ChannelRepository(RestClient client, ChannelMapper mapper)
{
    internal event Action<IChannel, ChangeType>? OnChange;
    internal readonly PermanentCache<IChannel, ChannelMapper> Cache = new(mapper);

    public async Task<IChannel> GetAsync(Snowflake id, bool bypassCache = false)
    {
        var channel = await Cache.GetOrCreateAsync(id, GetChannelRestAsync, bypassCache);
        
        OnChange?.Invoke(channel, ChangeType.Update);
        
        return channel;
    }

    internal IChannel Insert(ChannelResponse response) => Cache.UpdateOrCreate(mapper.FromDto(response));

    internal T? GetCachedOrDefault<T>(Snowflake id) where T : IChannel => Cache.GetCachedOrDefault<T>(id);
    
    private async Task<IChannel> GetChannelRestAsync(Snowflake id) 
        => await client.Channels[id].GetAsync() is {} channel 
            ? await mapper.FromDtoAsync(channel) 
            : throw new Exception($"Couldn't get channel with id {id}");

    public async Task<IChannel?> CreateAsync(Snowflake guildId)
    {
        var channelAsync = await client.Guilds[guildId].CreateChannelAsync(new ChannelCreateTextRequest("", null, null, null, null));
        if (channelAsync is null) return null;

        var entity = await mapper.FromDtoAsync(channelAsync);
        OnChange?.Invoke(entity, ChangeType.Create);
        return Cache.UpdateOrCreate(entity);
    }

    public async Task DeleteAsync(Snowflake id, bool silent = false)
    {
        await client.Channels[id].DeleteAsync(silent);
        Cache.Remove(id);
    }
    
    internal void Reset() => Cache.Clear();
}