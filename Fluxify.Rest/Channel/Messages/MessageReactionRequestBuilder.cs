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

namespace Fluxify.Rest.Channel.Messages;

public class MessageReactionRequestBuilder(HttpClient client, Snowflake channelId, Snowflake messageId, string emoji)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}/messages/{1}/reactions/{2}");
    private static readonly CompositeFormat ReactUrl = CompositeFormat.Parse("channels/{0}/messages/{1}/reactions/{2}/@me");
    private static readonly CompositeFormat ReactionUrl = CompositeFormat.Parse("channels/{0}/messages/{1}/reactions/{2}/{3}");
    private static string Uri(CompositeFormat format, Snowflake channelId, Snowflake messageId, string emoji) 
        => string.Format(FormatProvider, format, channelId, messageId, emoji);

    public Task<UserPartialResponse[]> ListUsersAsync(
        int? limit = null,
        Snowflake? after = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<UserPartialResponse[]>(
            HttpMethod.Get,
            Uri(GetUrl, channelId, messageId, emoji) + new QueryBuilder()
                .AddQuery("limit", limit?.ToString())
                .AddQuery("after", after?.ToString()),
            cancellationToken: cancellationToken
    );

    public Task ReactAsync(string sessionId, CancellationToken cancellationToken = default)
        => client.RequestAsync(
            HttpMethod.Put, 
            Uri(ReactUrl, channelId, messageId, emoji) + new QueryBuilder()
                .AddQuery("session_id", sessionId),
            cancellationToken: cancellationToken
        );
    
    public Task RemoveOwnReactionAsync(string sessionId, CancellationToken cancellationToken = default)
        => client.RequestAsync(
            HttpMethod.Delete, 
            Uri(ReactUrl, channelId, messageId, emoji) + new QueryBuilder()
                .AddQuery("session_id", sessionId),
            cancellationToken: cancellationToken
        );
    
    public Task RemoveReactionAsync(string targetId, CancellationToken cancellationToken = default)
        => client.RequestAsync(
            HttpMethod.Delete,
            string.Format(FormatProvider, ReactionUrl, channelId, messageId, emoji, targetId),
            cancellationToken: cancellationToken
        );
    
    public Task RemoveAllAsync(CancellationToken cancellationToken = default) 
        => client.RequestAsync(
            HttpMethod.Delete,
            Uri(GetUrl, channelId, messageId, emoji),
            cancellationToken: cancellationToken
        );
}