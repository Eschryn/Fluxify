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