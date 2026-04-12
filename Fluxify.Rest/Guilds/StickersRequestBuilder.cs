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
using Fluxify.Dto.Guilds.Stickers;

namespace Fluxify.Rest.Guilds;

public class StickersRequestBuilder(HttpClient client, Snowflake guildId)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat StickersUrl = CompositeFormat.Parse("guilds/{0}/stickers");
    private static readonly CompositeFormat StickersBulkUrl = CompositeFormat.Parse("guilds/{0}/stickers/bulk");
    private static readonly CompositeFormat StickerUrl = CompositeFormat.Parse("guilds/{0}/stickers/{1}");
    
    public Task<GuildStickerResponse[]> ListAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildStickerResponse[]>(
        HttpMethod.Get,
        string.Format(FormatProvider, StickersUrl, guildId),
        cancellationToken: cancellationToken
    );
    
    public Task<GuildStickerResponse> CreateAsync(
        GuildStickerCreateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildStickerCreateRequest, GuildStickerResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, StickersUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public Task<GuildStickerCreateBulkResponse> CreateBulkAsync(
        GuildStickerBulkCreateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildStickerBulkCreateRequest, GuildStickerCreateBulkResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, StickersBulkUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public Task DeleteAsync(
        Snowflake stickerId,
        bool? purge = null,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, StickerUrl, guildId, stickerId) + new QueryBuilder()
            .AddQuery("purge", purge?.ToString().ToLowerInvariant()),
        cancellationToken: cancellationToken
    );
    
    public Task<GuildStickerResponse> UpdateAsync(
        Snowflake stickerId,
        GuildStickerUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildStickerUpdateRequest, GuildStickerResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, StickerUrl, guildId, stickerId),
        request,
        cancellationToken: cancellationToken
    );
}