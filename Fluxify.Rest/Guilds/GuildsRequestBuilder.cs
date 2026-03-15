using Fluxify.Core.Types;
using Fluxify.Dto.Guilds;
using Fluxify.Rest.Channel;

namespace Fluxify.Rest.Guilds;

public class GuildsRequestBuilder(HttpClient client)
{
    private const string GuildsUrl = "guilds";
    private const string UserGuildsUrl = "users/@me/guilds";
    
    public GuildRequestBuilder this[Snowflake id] => new(client, id); 
    
    public async Task<GuildResponse?> CreateAsync(
        GuildCreateRequest guildCreateRequest,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildCreateRequest, GuildResponse>(
        HttpMethod.Post,
        GuildsUrl,
        guildCreateRequest,
        reason: reason,
        cancellationToken: cancellationToken
    );

    public async Task<GuildResponse[]?> ListAsync(
        Snowflake? before = null,
        Snowflake? after = null,
        int? limit = null,
        bool? withCounts = null,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildResponse[]>(
        HttpMethod.Get,
        UserGuildsUrl + new QueryBuilder()
            .AddQuery("before", before?.ToString())
            .AddQuery("after", after?.ToString())
            .AddQuery("limit", limit?.ToString())
            .AddQuery("with_counts", withCounts?.ToString().ToLowerInvariant()),
        cancellationToken: cancellationToken
    );
}