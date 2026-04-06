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

using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Members.Search;

namespace Fluxify.Rest.Guilds;

public class MembersRequestBuilder(HttpClient client, Snowflake guildId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat MembersUrl = CompositeFormat.Parse("guilds/{0}/members");
    private static readonly CompositeFormat MembersMeUrl = CompositeFormat.Parse("guilds/{0}/members/@me");
    private static readonly CompositeFormat SearchUrl = CompositeFormat.Parse("guilds/{0}/members-search");

    public MemberRequestBuilder this[Snowflake userId] => new(client, guildId, userId);

    public async Task<GuildMemberResponse[]?> ListMembersAsync(
        int? limit = null,
        Snowflake? after = null,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildMemberResponse[]>(
        HttpMethod.Get,
        string.Format(FormatProvider, MembersUrl, guildId) + new QueryBuilder()
            .AddQuery("limit", limit?.ToString())
            .AddQuery("after", after?.ToString()),
        cancellationToken: cancellationToken
    );

    public async Task<GuildMemberSearchResponse?> SearchAsync(
        GuildMemberSearchRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildMemberSearchRequest, GuildMemberSearchResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, SearchUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<GuildMemberResponse?> UpdateMeAsync(
        MyGuildMemberUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<MyGuildMemberUpdateRequest, GuildMemberResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, MembersMeUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
}