using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Rest.Guilds;

public class MemberRequestBuilder(HttpClient client, Snowflake guildId, Snowflake userId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat MemberUrl = CompositeFormat.Parse("guilds/{0}/members/{1}");
    private static readonly CompositeFormat RoleUrl = CompositeFormat.Parse("guilds/{0}/members/{1}/roles/{2}");
    
    public async Task<GuildMemberResponse?> GetAsync(CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<GuildMemberResponse>(
            HttpMethod.Get,
            string.Format(FormatProvider, MemberUrl, guildId, userId),
            cancellationToken: cancellationToken
        );

    public async Task KickAsync(
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, MemberUrl, guildId, userId),
        reason: reason,
        cancellationToken: cancellationToken
    );
    
    public async Task<GuildMemberResponse?> UpdateAsync(
        GuildMemberUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildMemberUpdateRequest, GuildMemberResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, MemberUrl, guildId, userId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task AddRoleAsync(
        Snowflake roleId,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Put,
        string.Format(FormatProvider, RoleUrl, guildId, userId, roleId),
        reason: reason,
        cancellationToken: cancellationToken
    );
    
    public async Task RemoveRoleAsync(
        Snowflake roleId,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, RoleUrl, guildId, userId, roleId),
        reason: reason,
        cancellationToken: cancellationToken
    );
}