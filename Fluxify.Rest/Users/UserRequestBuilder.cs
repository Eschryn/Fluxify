using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Users;
using Fluxify.Rest.Channel;

namespace Fluxify.Rest.Users;

public class UserRequestBuilder(HttpClient client, Snowflake userId)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

    private static readonly CompositeFormat UserUrl = CompositeFormat.Parse("users/{0}");
    private static readonly CompositeFormat UserProfileUrl = CompositeFormat.Parse("users/{0}/profile");

    public async Task<UserProfileFullResponse?> GetUserProfileAsync(
        Snowflake? guildId = null,
        bool withMutualFriends = false,
        bool withMutualGuilds = false,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<UserProfileFullResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, UserProfileUrl, userId) + new QueryBuilder()
            .AddQuery("guild_id", guildId?.ToString())
            .AddQuery("with_mutual_friends", withMutualFriends.ToString().ToLowerInvariant())
            .AddQuery("with_mutual_guilds", withMutualGuilds.ToString().ToLowerInvariant()),
        cancellationToken: cancellationToken
    );
    
    public async Task<UserPartialResponse?> GetAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<UserPartialResponse>(
        HttpMethod.Get,
        string.Format(FormatProvider, UserUrl, userId),
        cancellationToken: cancellationToken
    );
}