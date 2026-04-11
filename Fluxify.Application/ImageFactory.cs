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

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Fluxify.Application.Common;

namespace Fluxify.Application;

internal class ImageFactory(FluxerApplication app)
{
    private static CompositeFormat IconUriFormat => CompositeFormat.Parse("/icons/{0}/{1}");
    private static CompositeFormat BannersUriFormat => CompositeFormat.Parse("/banners/{0}/{1}");
    private static CompositeFormat AvatarsUriFormat => CompositeFormat.Parse("/avatars/{0}/{1}");
    private static CompositeFormat SplashesUriFormat => CompositeFormat.Parse("/splashes/{0}/{1}");
    private static CompositeFormat EmbedSplashesUriFormat => CompositeFormat.Parse("/embed-splashes/{0}/{1}");
    private static CompositeFormat MemberAvatarUriFormat => CompositeFormat.Parse("/guilds/{0}/users/{1}/avatars/{2}");
    private static CompositeFormat MemberBannerUriFormat => CompositeFormat.Parse("/guilds/{0}/users/{1}/banners/{2}");
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(hash))]
    public Image? MakeIcon(Snowflake guildId, MediaHash? hash)
        => MakeImage(guildId, IconUriFormat, hash, null, null);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(hash))]
    public Image? MakeBanner(Snowflake targetId, MediaHash? hash, int? width, int? height)
        => MakeImage(targetId, BannersUriFormat, hash, width, height);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(hash))]
    public Image? MakeAvatar(Snowflake targetId, MediaHash? hash, int? width, int? height)
        => MakeImage(targetId, AvatarsUriFormat, hash, width, height);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(hash))]
    public Image? MakeSplash(Snowflake guildId, MediaHash? hash, int? width, int? height)
        => MakeImage(guildId, SplashesUriFormat, hash, width, height);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(hash))]
    public Image? MakeEmbedSplash(Snowflake guildId, MediaHash? hash, int? width, int? height)
        => MakeImage(guildId, EmbedSplashesUriFormat, hash, width, height);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(hash))]
    public Image? MakeMemberAvatar(Snowflake userId, Snowflake guildId, MediaHash? hash, int? width, int? height)
        => MakeImage(guildId, userId, MemberAvatarUriFormat, hash, width, height);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(hash))]
    public Image? MakeMemberBanner(Snowflake userId, Snowflake guildId, MediaHash? hash, int? width, int? height)
        => MakeImage(guildId, userId, MemberBannerUriFormat, hash, width, height);
    
    [return: NotNullIfNotNull(nameof(hash))]
    private Image? MakeImage(
        Snowflake targetId,
        CompositeFormat template,
        MediaHash? hash,
        int? width,
        int? height
    ) => hash == null
        ? null
        : new Image(
            new Uri(
                app.InstanceInfo?.Endpoints.Media ?? throw new InvalidOperationException(),
                string.Format(CultureInfo.InvariantCulture, template, targetId, hash)
            ),
            width,
            height
        );
    [return: NotNullIfNotNull(nameof(hash))]
    private Image? MakeImage(
        Snowflake guildId,
        Snowflake targetId,
        CompositeFormat template,
        MediaHash? hash,
        int? width,
        int? height
    ) => hash == null
        ? null
        : new Image(
            new Uri(
                app.InstanceInfo?.Endpoints.Media ?? throw new InvalidOperationException(),
                string.Format(CultureInfo.InvariantCulture, template, guildId, targetId, hash)
            ),
            width,
            height
        );
}