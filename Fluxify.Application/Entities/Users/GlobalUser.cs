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
using System.Globalization;
using System.Text;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Model;
using Fluxify.Core.Types;
using Fluxify.Dto.Common;

namespace Fluxify.Application.Entities.Users;

public class GlobalUser(FluxerApplication fluxerApplication) : IUser, IPresence
{
    private static readonly CompositeFormat FallbackAvatarUriFormat = CompositeFormat.Parse("/avatars/{0}.png");
    private static readonly CompositeFormat AvatarUriFormat =
            CompositeFormat.Parse("/avatars/{0}/{1}.{2}?size={3}&format={2}&quality={4}&animated={5}");
        private static readonly CompositeFormat BannersUriFormat =
            CompositeFormat.Parse("/banners/{0}/{1}.{2}?size={3}&format={2}&quality={4}&animated={5}");

    public UserStatus Status { get; internal set; } = UserStatus.Offline;
    public bool IsMobile { get; internal set; } = false;
    public bool IsAfk { get; internal set; } = false;
    public CustomStatus? CustomStatus { get; internal set; } = null;
    public Snowflake Id { get; init; }
    public bool? Bot { get; internal set; }
    public string Username { get; internal set; } = string.Empty;
    public string? Discriminator { get; internal set; }
    public string? GlobalName { get; internal set; }
    public MediaHash? AvatarHash { get; internal set; }
    public MediaHash? BannerHash { get; internal set; }
    public Color? AvatarColor { get; internal set; }
    public bool? System { get; internal set; }
    public PublicUserFlags Flags { get; internal set; }
    
    public Task<Dm> GetOrCreateDmAsync(CancellationToken cancellationToken = default) 
        => fluxerApplication.GetOrCreateDmAsync(Id, cancellationToken);

    public Uri GetAvatarUri(
        int size = 128,
        ImageFormat format = ImageFormat.Webp,
        ImageQuality quality = ImageQuality.High,
        bool animated = false
    ) => AvatarHash != null ? new Uri(
        fluxerApplication.InstanceInfo!.Endpoints.Media,
        string.Format(
            CultureInfo.InvariantCulture,
            AvatarUriFormat,
            Id,
            AvatarHash?.Hash,
            format.ToString().ToLowerInvariant(),
            size,
            quality.ToString().ToLowerInvariant(),
            animated.ToString().ToLowerInvariant()
        )
    ) : new Uri(
        fluxerApplication.InstanceInfo!.Endpoints.StaticCdn,
        string.Format(
            CultureInfo.InvariantCulture,
            FallbackAvatarUriFormat,
            ((ulong)Id) % 6
        )
    );
    
    public Uri? GetBannerUri(
        int size = 128,
        ImageFormat format = ImageFormat.Webp,
        ImageQuality quality = ImageQuality.High,
        bool animated = false
    ) => BannerHash != null ? new(
        fluxerApplication.InstanceInfo!.Endpoints.Media,
        string.Format(
            CultureInfo.InvariantCulture,
            BannersUriFormat,
            Id,
            AvatarHash?.Hash,
            format.ToString().ToLowerInvariant(),
            size,
            quality.ToString().ToLowerInvariant(),
            animated.ToString().ToLowerInvariant()
        )
    ) : null;

    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        "i" or "I" => ((long)Id).ToString(),
        _ or "m" or "M" => $"<#{Id}>"
    };
}