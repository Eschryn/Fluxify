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
using Fluxify.Dto.Guilds.Emoji;

namespace Fluxify.Rest.Guilds;

public class EmojisRequestBuilder(HttpClient client, Snowflake guildId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat EmojisUrl = CompositeFormat.Parse("guilds/{0}/emojis");
    private static readonly CompositeFormat EmojiUrl = CompositeFormat.Parse("guilds/{0}/emojis/{1}");
    private static readonly CompositeFormat BulkEmojisUrl = CompositeFormat.Parse("guilds/{0}/emojis/bulk");
    
    public Task<GuildEmojiResponse> CreateAsync(
        GuildEmojiCreateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildEmojiCreateRequest, GuildEmojiResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, EmojisUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public Task<GuildEmojiBulkCreateResponse> BulkCreateAsync(
        GuildEmojiBulkCreateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildEmojiBulkCreateRequest, GuildEmojiBulkCreateResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, BulkEmojisUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public Task DeleteEmojiAsync(
        Snowflake emojiId,
        bool? purge = null,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, EmojiUrl, guildId, emojiId) + new QueryBuilder()
            .AddQuery("purge", purge?.ToString().ToLowerInvariant()),
        cancellationToken: cancellationToken
    );

    public Task UpdateEmojiAsync(
        Snowflake emojiId,
        GuildEmojiUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Patch,
        string.Format(FormatProvider, EmojiUrl, guildId, emojiId),
        request,
        cancellationToken: cancellationToken
    );
}