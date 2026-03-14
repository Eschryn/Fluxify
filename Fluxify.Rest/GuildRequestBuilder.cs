using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Guilds;

namespace Fluxify.Rest;

public class GuildRequestBuilder(HttpClient client, Snowflake guildId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetGuildUrl = CompositeFormat.Parse("guilds/{0}");
    private static readonly CompositeFormat ChannelsUrl = CompositeFormat.Parse("guilds/{0}/channels");

    public Task<GuildResponse?> GetAsync() 
        => client.GetFromJsonAsync<GuildResponse>(string.Format(FormatProvider, GetGuildUrl, guildId));

    public async Task<ChannelResponse?> CreateChannelAsync(ChannelCreateRequest channelCreateRequest) 
        => await client.JsonRequestAsync<ChannelCreateRequest, ChannelResponse>(
            HttpMethod.Post,
            string.Format(FormatProvider, ChannelsUrl, guildId),
            channelCreateRequest
        );
}