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

using System.Runtime.CompilerServices;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.Model;
using Fluxify.Application.Model.Guild;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Application.Entities.Guilds.Members;

public partial class GuildMember
{
    public Task AddRoleAsync(IRole role, string? reason = null, CancellationToken cancellationToken = default)
        => RequestBuilder.AddRoleAsync(role.Id, reason, cancellationToken);

    public Task AddRoleAsync(Snowflake roleId, string? reason = null, CancellationToken cancellationToken = default)
        => RequestBuilder.AddRoleAsync(roleId, reason, cancellationToken);

    public Task RemoveRoleAsync(IRole role, string? reason = null, CancellationToken cancellationToken = default)
        => RequestBuilder.RemoveRoleAsync(role.Id, reason, cancellationToken);

    public Task RemoveRoleAsync(Snowflake roleId, string? reason = null, CancellationToken cancellationToken = default)
        => RequestBuilder.RemoveRoleAsync(roleId, reason, cancellationToken);

    public Task KickAsync(string? reason = null, CancellationToken cancellationToken = default)
        => RequestBuilder.KickAsync(reason, cancellationToken);

    public Task<Dm> GetOrCreateDmAsync(CancellationToken cancellationToken = default)
        => _fluxerApplication.GetOrCreateDmAsync(Id, cancellationToken);

    public Task BanAsync(
        int? deleteMessageDays = null,
        TimeSpan? banDuration = null,
        string? banReason = null,
        string? auditLogReason = null,
        CancellationToken cancellationToken = default
    ) => Guild.BanAsync(Id, deleteMessageDays, banDuration, banReason, auditLogReason, cancellationToken);

    public Task UnbanAsync(
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => Guild.UnbanAsync(Id, reason, cancellationToken);

    public async Task<IGuildMember> UpdateAsync(
        Action<GuildMemberProperties> update,
        string? reason = null,
        CancellationToken cancellationToken = default
    )
    {
        var updatePayload = new GuildMemberProperties();
        update(updatePayload);

        return await CallUpdateAsync(
            cancellationToken,
            reason,
            _fluxerApplication.MemberMapper.MapToRequest(updatePayload)
        );
    }

    public async Task<IGuildMember> SetNicknameAsync(
        string? nickname,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => await CallUpdateAsync(
        cancellationToken,
        reason,
        new GuildMemberUpdateRequest(Nick: nickname)
    );

    public async Task<IGuildMember> SetTimeoutAsync(
        DateTimeOffset until,
        string? timeoutReason = null,
        string? auditLogReason = null,
        CancellationToken cancellationToken = default
    ) => await CallUpdateAsync(
        cancellationToken,
        auditLogReason,
        new GuildMemberUpdateRequest(CommunicationsDisabledUntil: until, TimeoutReason: timeoutReason)
    );

    public async Task<IGuildMember> RemoveTimeoutAsync(
        string? reason,
        CancellationToken cancellationToken = default
    ) => await CallUpdateAsync(
        cancellationToken,
        reason,
        new GuildMemberUpdateRequest(TimeoutReason: null)
    );

    public async Task<IGuildMember> UpdateRolesAsync(
        Func<IReadOnlyCollection<IRole>, IEnumerable<Snowflake>> rolesSelector,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => await CallUpdateAsync(
        cancellationToken,
        reason,
        new GuildMemberUpdateRequest(Roles: rolesSelector(Roles).ToArray())
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async Task<IGuildMember> CallUpdateAsync(
        CancellationToken cancellationToken,
        string? reason,
        GuildMemberUpdateRequest request
    )
    {
        var response = await RequestBuilder.UpdateAsync(
            request,
            reason,
            cancellationToken
        );

        return Guild.MembersRepository.Insert(response, GuildRef, UserRef).Value!;
    }

    public Uri GetAvatarUri(
        int size = 128,
        ImageFormat format = ImageFormat.Webp,
        ImageQuality quality = ImageQuality.High,
        bool animated = false
    ) => Avatar?.GetUri(size, format, quality, animated)
         ?? User.GetAvatarUri(size, format, quality, animated);

    public Uri? GetBannerUri(
        int size = 128,
        ImageFormat format = ImageFormat.Webp,
        ImageQuality quality = ImageQuality.High,
        bool animated = false
    ) => Banner?.GetUri(size, format, quality, animated);
}