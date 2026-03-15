using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Stickers;
using Fluxify.Rest.Channel;

namespace Fluxify.Rest.Guilds;

public class StickersRequestBuilder(HttpClient client, Snowflake guildId)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat StickersUrl = CompositeFormat.Parse("guilds/{0}/stickers");
    private static readonly CompositeFormat StickersBulkUrl = CompositeFormat.Parse("guilds/{0}/stickers/bulk");
    private static readonly CompositeFormat StickerUrl = CompositeFormat.Parse("guilds/{0}/stickers/{1}");
    
    public async Task<GuildStickerResponse[]?> ListAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildStickerResponse[]>(
        HttpMethod.Get,
        string.Format(FormatProvider, StickersUrl, guildId),
        cancellationToken: cancellationToken
    );
    
    public async Task<GuildStickerResponse?> CreateAsync(
        GuildStickerCreateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildStickerCreateRequest, GuildStickerResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, StickersUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<GuildStickerCreateBulkResponse?> CreateBulkAsync(
        GuildStickerBulkCreateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildStickerBulkCreateRequest, GuildStickerCreateBulkResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, StickersBulkUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task DeleteAsync(
        Snowflake stickerId,
        bool? purge = null,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, StickerUrl, guildId, stickerId) + new QueryBuilder()
            .AddQuery("purge", purge?.ToString().ToLowerInvariant()),
        cancellationToken: cancellationToken
    );
    
    public async Task<GuildStickerResponse?> UpdateAsync(
        Snowflake stickerId,
        GuildStickerUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildStickerUpdateRequest, GuildStickerResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, StickerUrl, guildId, stickerId),
        request,
        cancellationToken: cancellationToken
    );
}