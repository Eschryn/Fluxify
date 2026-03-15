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