using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Members.Search;
using Fluxify.Rest.Channel;

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