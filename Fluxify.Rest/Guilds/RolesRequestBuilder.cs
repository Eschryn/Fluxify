using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Roles;

namespace Fluxify.Rest.Guilds;

public class RolesRequestBuilder(HttpClient client, Snowflake guildId)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat RolesUrl = CompositeFormat.Parse("guilds/{0}/roles");
    private static readonly CompositeFormat RolesHoistUrl = CompositeFormat.Parse("guilds/{0}/roles/hoist-positions");

    private static string Uri(CompositeFormat format, Snowflake guildId) =>
        string.Format(FormatProvider, format, guildId);

    public async Task<GuildRoleResponse[]?> ListAsync(CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<GuildRoleResponse[]>(
            HttpMethod.Get,
            Uri(RolesUrl, guildId),
            cancellationToken: cancellationToken
        );

    public async Task<GuildRoleResponse?> CreateAsync(
        GuildRoleCreateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildRoleCreateRequest, GuildRoleResponse>(
        HttpMethod.Post,
        Uri(RolesUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task UpdatePositionAsync(
        GuildRolePositionItem request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Patch,
        Uri(RolesUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task ResetHoistPositionAsync(CancellationToken cancellationToken = default)
        => await client.RequestAsync(
            HttpMethod.Delete,
            Uri(RolesHoistUrl, guildId),
            cancellationToken: cancellationToken
        );

    public async Task UpdateHoistPosition(
        GuildRoleHoistPositionItem request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Patch,
        Uri(RolesHoistUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<GuildRoleResponse?> UpdateAsync(
        Snowflake roleId,
        GuildRoleUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<GuildRoleUpdateRequest, GuildRoleResponse>(
        HttpMethod.Patch,
        Uri(RolesUrl, guildId),
        request,
        cancellationToken: cancellationToken
    );

    public async Task DeleteAsync(
        Snowflake roleId,
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, RolesUrl, guildId, roleId),
        cancellationToken: cancellationToken
    );
}