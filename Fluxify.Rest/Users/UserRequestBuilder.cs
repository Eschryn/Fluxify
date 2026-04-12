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
using Fluxify.Dto.Users;

namespace Fluxify.Rest.Users;

public class UserRequestBuilder(HttpClient client, Snowflake userId)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

    private static readonly CompositeFormat UserUrl = CompositeFormat.Parse("users/{0}");
    private static readonly CompositeFormat UserProfileUrl = CompositeFormat.Parse("users/{0}/profile");

    public Task<UserProfileFullResponse> GetProfileAsync(
        Snowflake? guildId = null,
        bool withMutualFriends = false,
        bool withMutualGuilds = false,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<UserProfileFullResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, UserProfileUrl, userId) + new QueryBuilder()
            .AddQuery("guild_id", guildId?.ToString())
            .AddQuery("with_mutual_friends", withMutualFriends.ToString().ToLowerInvariant())
            .AddQuery("with_mutual_guilds", withMutualGuilds.ToString().ToLowerInvariant()),
        cancellationToken: cancellationToken
    );
    
    public Task<UserPartialResponse> GetAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<UserPartialResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, UserUrl, userId),
        cancellationToken: cancellationToken
    );
}