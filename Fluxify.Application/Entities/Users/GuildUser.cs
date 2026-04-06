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

using System.Collections.Concurrent;
using System.Drawing;
using System.Globalization;
using System.Text;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.Model;
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;
using Fluxify.Dto.Common;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Rest.Guilds;

namespace Fluxify.Application.Entities.Users;

public class GuildMember(FluxerApplication fluxerApplication) : IGuildMember
{
    private static readonly CompositeFormat AvatarUriFormat = CompositeFormat.Parse("/guilds/{0}/users/{1}/avatars/{2}.{3}?size={4}&format={3}&quality={5}&animated={6}");
    private static readonly CompositeFormat BannerUriFormat = CompositeFormat.Parse("/guilds/{0}/users/{1}/banners/{2}.{3}?size={4}&format={3}&quality={5}&animated={6}");
    
    private MemberRequestBuilder RequestBuilder => field ??= Guild.RequestBuilder.Members[Id];
    
    internal CacheRef<Guild> GuildRef { get; init; }
    internal CacheRef<GlobalUser> UserRef { get; set; }
    
    public Snowflake Id => UserRef.Id;
    public Snowflake[] AssignedRoleIds { get; set; } = [];
    public Color? AccentColor { get; internal set;  }

    public MediaHash? AvatarHash
    {
        get => field ?? User.AvatarHash;
        internal set;
    }

    public MediaHash? BannerHash
    {
        get => field ??= User.BannerHash;
        internal set;
    } 
    public DateTimeOffset? JoinedAt { get; internal set;  }
    public DateTimeOffset? CommunicationsDisabledUntil { get; internal set;  }
    public bool Deaf { get; internal set;  }
    public bool Mute { get; internal set;  }
    public string? Nick { get; internal set;  }
    public GuildMemberProfileFlags ProfileFlags { get; internal set;  }
    public IReadOnlyCollection<IRole> Roles => AssignedRoleIds.Select(r => Guild.RolesRepository.Cache.GetCachedOrDefault(r).Value).OfType<IRole>().ToArray();
    public Guild Guild => GuildRef.Value;
    public GlobalUser User => UserRef.Value;
    public UserStatus Status => User.Status;
    public bool IsMobile => User.IsMobile;
    public bool IsAfk => User.IsAfk;
    public CustomStatus? CustomStatus => User.CustomStatus;
    public bool? Bot => User.Bot;
    public string Username => User.Username;
    public string? Discriminator => User.Discriminator;
    public string? GlobalName => User.GlobalName;
    MediaHash? IUser.AvatarHash => User.AvatarHash;
    public Color? AvatarColor => User.AvatarColor;
    public bool? System => User.System;
    public PublicUserFlags Flags => User.Flags;

    internal readonly ConcurrentDictionary<string, VoiceState> VoiceStateList = [];
    public IReadOnlyCollection<IVoiceState> VoiceStates => VoiceStateList.Values.Cast<IVoiceState>().ToArray();

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
        => fluxerApplication.GetOrCreateDmAsync(Id, cancellationToken);
    
    public Uri GetAvatarUri(
        int size = 128,
        ImageFormat format = ImageFormat.Webp,
        ImageQuality quality = ImageQuality.High,
        bool animated = false
    )
    {
        if (User.AvatarHash == AvatarHash)
        {
            return User.GetAvatarUri(size, format, quality, animated);
        }

        return new Uri(
            fluxerApplication.InstanceInfo!.Endpoints.Media,
            string.Format(
                CultureInfo.InvariantCulture,
                AvatarUriFormat,
                Guild.Id,
                Id,
                AvatarHash?.Hash,
                format.ToString().ToLowerInvariant(),
                size,
                quality.ToString().ToLowerInvariant(),
                animated.ToString().ToLowerInvariant()
            )
        );
    }
    
    public Uri? GetBannerUri(
        int size = 128,
        ImageFormat format = ImageFormat.Webp,
        ImageQuality quality = ImageQuality.High,
        bool animated = false
    )
    {
        if (User.BannerHash == BannerHash)
        {
            return User.GetBannerUri(size, format, quality, animated);
        }

        return new Uri(
            fluxerApplication.InstanceInfo!.Endpoints.Media,
            string.Format(
                CultureInfo.InvariantCulture,
                BannerUriFormat,
                Guild.Id,
                Id,
                AvatarHash?.Hash,
                format.ToString().ToLowerInvariant(),
                size,
                quality.ToString().ToLowerInvariant(),
                animated.ToString().ToLowerInvariant()
            )
        );
    }
    
    public string ToString(string? format, IFormatProvider? formatProvider) => User.ToString(format, formatProvider);
    public object Clone() => MemberwiseClone();
}