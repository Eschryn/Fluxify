using Fluxify.Application.Entities.Channels;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Text;
using Fluxify.Rest;

namespace Fluxify.Application.State;

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
        if (await client.Channels[id].DeleteAsync(silent))
        {
            _cache.Remove(id);   
        }
    }
    
    internal void Reset() => _cache.Clear();
}