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

/// <summary>
/// Exposes requests related to the member specified by <paramref name="userId"/> in the guild <paramref name="guildId"/>.
/// </summary>
/// <param name="client">The client that should be used to send requests with.</param>
/// <param name="guildId">The id of the guild that the members are scoped to.</param>
/// <param name="userId">The id of the member that the requests are scoped to.</param>
public class MemberRequestBuilder(HttpClient client, Snowflake guildId, Snowflake userId)
{
    private static readonly CultureInfo FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat MemberUrl = CompositeFormat.Parse("guilds/{0}/members/{1}");
    private static readonly CompositeFormat RoleUrl = CompositeFormat.Parse("guilds/{0}/members/{1}/roles/{2}");

    /// <summary>
    /// Gets the guild member profile.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="NotPermittedException">This exception is thrown when the caller is not a member of the guild or the guild is unavailable.</exception>
    /// <exception cref="NotFoundException">This exception is thrown when the guild does not exist or the target is not a member.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <seealso href="https://docs.fluxer.app/http-api/guild-members/#get-guild-member"/>
    public Task<GuildMemberResponse> GetAsync(CancellationToken cancellationToken = default)
        => client.JsonRequestAsync<GuildMemberResponse>(
            HttpMethod.Get,
            string.Format(FormatProvider, MemberUrl, guildId, userId),
            DtoJsonContext.Default.GuildMemberResponse,
            bucket: RateLimitDefaults.GuildMembers,
            cancellationToken: cancellationToken
        );

    /// <summary>
    /// Removes a member from a guild.
    /// </summary>
    /// <param name="reason">The reason for why the user was kicked. Will be shown in the audit log.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="NotPermittedException">This exception is thrown when the caller is not a member of the guild or does not have the <see cref="Permissions.KickMembers"/> permission.</exception>
    /// <exception cref="NotFoundException">This exception is thrown when the target is not a member, is the caller or is the guild owner or the guild does not exist.</exception>
    /// <exception cref="RestApiException">This exception is thrown when the caller has <see cref="Permissions.KickMembers"/> and the guild requires mfa but the caller doesn't have mfa enabled.</exception>
    /// <exception cref="RateLimitException">This exception is thrown when a rate limit has been hit without the <see cref="FluxerRateLimitingHandler"/>.</exception>
    /// <remarks>
    /// This operation requires the permission <see cref="Permissions.KickMembers"/>.<br/>
    /// This operation cannot be used on the caller or the guild owner.
    /// </remarks>
    /// <seealso href="https://docs.fluxer.app/http-api/guild-members/#remove-guild-member"/>
    public Task KickAsync(
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.RequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, MemberUrl, guildId, userId),
        reason: reason,
        bucket: RateLimitDefaults.GuildMemberRemove,
        cancellationToken: cancellationToken
    );

    public Task<GuildMemberResponse> UpdateAsync(
        GuildMemberUpdateRequest request,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<GuildMemberUpdateRequest, GuildMemberResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, MemberUrl, guildId, userId),
        request,
        DtoJsonContext.Default.GuildMemberUpdateRequest,
        DtoJsonContext.Default.GuildMemberResponse,
        reason,
        bucket: RateLimitDefaults.GuildMemberUpdate,
        cancellationToken
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