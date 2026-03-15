using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Emoji;
using Fluxify.Rest.Channel;

namespace Fluxify.Rest.Guilds;

public class EmojisRequestBuilder(HttpClient client, Snowflake guildId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat EmojisUrl = CompositeFormat.Parse("guilds/{0}/emojis");
    private static readonly CompositeFormat EmojiUrl = CompositeFormat.Parse("guilds/{0}/emojis/{1}");
    private static readonly CompositeFormat BulkEmojisUrl = CompositeFormat.Parse("guilds/{0}/emojis/bulk");
    
    public async Task<GuildEmojiResponse?> CreateAsync(
        GuildEmojiCreateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildEmojiCreateRequest, GuildEmojiResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, EmojisUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<GuildEmojiBulkCreateResponse?> BulkCreateAsync(
        GuildEmojiBulkCreateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildEmojiBulkCreateRequest, GuildEmojiBulkCreateResponse>(
        HttpMethod.Post,
        string.Format(FormatProvider, BulkEmojisUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task DeleteEmojiAsync(
        Snowflake emojiId,
        bool? purge = null,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, EmojiUrl, guildId, emojiId) + new QueryBuilder()
            .AddQuery("purge", purge?.ToString().ToLowerInvariant()),
        cancellationToken: cancellationToken
    );

    public async Task UpdateEmojiAsync(
        Snowflake emojiId,
        GuildEmojiUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Patch,
        string.Format(FormatProvider, EmojiUrl, guildId, emojiId),
        request,
        cancellationToken: cancellationToken
    );
}