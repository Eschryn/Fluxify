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
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Rest.Guilds;

public class MemberRequestBuilder(HttpClient client, Snowflake guildId, Snowflake userId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat MemberUrl = CompositeFormat.Parse("guilds/{0}/members/{1}");
    private static readonly CompositeFormat RoleUrl = CompositeFormat.Parse("guilds/{0}/members/{1}/roles/{2}");
    
    public Task<GuildMemberResponse> GetAsync(CancellationToken cancellationToken = default)
        => client.JsonRequestAsync<GuildMemberResponse>(
            HttpMethod.Get,
            string.Format(FormatProvider, MemberUrl, guildId, userId),
            cancellationToken: cancellationToken
        );

    public Task KickAsync(
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, MemberUrl, guildId, userId),
        reason: reason,
        cancellationToken: cancellationToken
    );
    
    public Task<GuildMemberResponse> UpdateAsync(
        GuildMemberUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildMemberUpdateRequest, GuildMemberResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, MemberUrl, guildId, userId),
        request,
        cancellationToken: cancellationToken
    );
    
    public Task AddRoleAsync(
        Snowflake roleId,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Put,
        string.Format(FormatProvider, RoleUrl, guildId, userId, roleId),
        reason: reason,
        cancellationToken: cancellationToken
    );
    
    public Task RemoveRoleAsync(
        Snowflake roleId,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, RoleUrl, guildId, userId, roleId),
        reason: reason,
        cancellationToken: cancellationToken
    );
}