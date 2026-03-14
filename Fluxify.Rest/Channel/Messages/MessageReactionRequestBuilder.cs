using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Rest.Channel;

public class MessageReactionRequestBuilder(HttpClient client, Snowflake channelId, Snowflake messageId, string emoji)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GetUrl = CompositeFormat.Parse("channels/{0}/messages/{1}/reactions/{2}");
    private static readonly CompositeFormat ReactUrl = CompositeFormat.Parse("channels/{0}/messages/{1}/reactions/{2}/@me");
    private static readonly CompositeFormat ReactionUrl = CompositeFormat.Parse("channels/{0}/messages/{1}/reactions/{2}/{3}");
    private static string Uri(CompositeFormat format, Snowflake channelId, Snowflake messageId, string emoji) 
        => string.Format(FormatProvider, format, channelId, messageId, emoji);

    public async Task<UserPartialResponse[]?> ListUsersAsync()
        => await client.JsonRequestAsync<UserPartialResponse[]>(
            HttpMethod.Get,
            Uri(GetUrl, channelId, messageId, emoji)
        );

    public async Task ReactAsync(string sessionId, CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Put, 
            Uri(ReactUrl, channelId, messageId, emoji) + new QueryBuilder()
                .AddQuery("session_id", sessionId),
            cancellationToken: cancellationToken
        );
    
    public async Task RemoveOwnReactionAsync(string sessionId, CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Delete, 
            Uri(ReactUrl, channelId, messageId, emoji) + new QueryBuilder()
                .AddQuery("session_id", sessionId),
            cancellationToken: cancellationToken
        );
    
    public async Task RemoveReactionAsync(string targetId, CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Delete,
            string.Format(FormatProvider, ReactionUrl, channelId, messageId, emoji, targetId),
            cancellationToken: cancellationToken
        );
    
    public async Task RemoveAllAsync(CancellationToken cancellationToken = default) 
        => await client.RequestAsync(
            HttpMethod.Delete,
            Uri(GetUrl, channelId, messageId, emoji),
            cancellationToken: cancellationToken
        );
}