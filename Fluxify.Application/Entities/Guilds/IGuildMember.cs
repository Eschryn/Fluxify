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

using System.Drawing;
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model;
using Fluxify.Application.Model.Guild;
using Fluxify.Application.State;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Application.Entities.Guilds;

public interface IGuildMember : IUser, ICloneable<IGuildMember>
{
    Color? AccentColor { get; }
    DateTimeOffset? JoinedAt { get; }
    DateTimeOffset? CommunicationsDisabledUntil { get; }
    bool Deaf { get; }
    bool Mute { get; }
    string? Nick { get; }
    GuildMemberProfileFlags ProfileFlags { get; }
    IReadOnlyCollection<IRole> Roles { get; }
    Guild Guild { get; }
    IReadOnlyCollection<IVoiceState> VoiceStates { get; }
    Task AddRoleAsync(IRole role, string? reason = null, CancellationToken cancellationToken = default);
    Task AddRoleAsync(Snowflake roleId, string? reason = null, CancellationToken cancellationToken = default);
    Task RemoveRoleAsync(IRole role, string? reason = null, CancellationToken cancellationToken = default);
    Task RemoveRoleAsync(Snowflake roleId, string? reason = null, CancellationToken cancellationToken = default);
    Task KickAsync(string? reason = null, CancellationToken cancellationToken = default);

    Uri? GetBannerUri(
        int size = 128,
        ImageFormat format = ImageFormat.Webp,
        ImageQuality quality = ImageQuality.High,
        bool animated = false
    );

    Task BanAsync(
        int? deleteMessageDays = null,
        // min 60s max 2years
        TimeSpan? banDuration = null,
        string? banReason = null,
        string? auditLogReason = null,
        CancellationToken cancellationToken = default
    );

    Task UnbanAsync(
        string? reason = null,
        CancellationToken cancellationToken = default
    );

    Task<IGuildMember> UpdateAsync(
        Action<GuildMemberProperties> update,
        string? reason = null,
        CancellationToken cancellationToken = default
    );

    Task<IGuildMember> SetNicknameAsync(
        string? nickname,
        string? reason = null,
        CancellationToken cancellationToken = default
    );

    Task<IGuildMember> SetTimeoutAsync(
        DateTimeOffset until,
        string? timeoutReason = null,
        string? auditLogReason = null,
        CancellationToken cancellationToken = default
    );

    Task<IGuildMember> RemoveTimeoutAsync(
        string? reason,
        CancellationToken cancellationToken = default
    );

    Task<IGuildMember> UpdateRolesAsync(
        Func<IReadOnlyCollection<IRole>, IEnumerable<Snowflake>> rolesSelector,
        string? reason = null,
        CancellationToken cancellationToken = default
    );
}