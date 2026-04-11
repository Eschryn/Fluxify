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
using Fluxify.Application.Common;
using Fluxify.Application.Model.Guild;
using Fluxify.Application.State;
using Fluxify.Dto;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.Invite;
using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Application.Entities.Guilds;

[Mapper]
internal partial class GuildMapper(FluxerApplication app) 
    : IUpdateEntity<Guild, GuildResponse>,
        ICreateEntity<Guild, GuildResponse>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private FluxerApplication App() => app;

    [UseMapper] private ImageFactory ImageFactory { get; } = app.ImageFactory;
    [UseMapper] private CacheMapper CacheMapper { get; } = app.CacheMapper;

    private static GuildOperations ToOperations(long operations) => (GuildOperations)operations;

    [NamedMapping("MapGuildFromResponse")]
    [MapValue("app", Use = nameof(App))]
    [MapPropertyFromSource(nameof(Guild.Icon), Use = nameof(CreateIcon))]
    [MapPropertyFromSource(nameof(Guild.Banner), Use = nameof(CreateBanner))]
    [MapPropertyFromSource(nameof(Guild.Splash), Use = nameof(CreateSplash))]
    [MapPropertyFromSource(nameof(Guild.EmbedSplash), Use = nameof(CreateEmbedSplash))]
    [MapProperty(nameof(GuildResponse.OwnerId), nameof(Guild.OwnerRef))]
    [MapProperty(nameof(GuildResponse.AfkChannelId), nameof(Guild.AfkChannelRef))]
    [MapProperty(nameof(GuildResponse.RulesChannelId), nameof(Guild.RulesChannelRef))]
    [MapProperty(nameof(GuildResponse.SystemChannelId), nameof(Guild.SystemChannelRef))]
    public partial Guild MapFromResponse(GuildResponse dto);

    [IncludeMappingConfiguration("MapGuildFromResponse")]
    public partial void UpdateEntity([MappingTarget] Guild data, GuildResponse update);

    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    [MapValue(nameof(GuildProperties.Icon), null)]
    [MapValue(nameof(GuildProperties.EmbedSplash), null)]
    [MapValue(nameof(GuildProperties.Splash), null)]
    [MapValue(nameof(GuildProperties.Banner), null)]
    [MapProperty(nameof(Guild.AfkChannelRef), nameof(GuildProperties.AfkChannelId))]
    [MapProperty(nameof(Guild.SystemChannelRef), nameof(GuildProperties.SystemChannelId))]
    public partial GuildProperties ToProperties(Guild guild);

    public partial GuildUpdateRequest ToUpdateRequest(
        GuildProperties guildProperties,
        string? mfaCode = null,
        MfaMethod? mfaMethod = null,
        string? password = null,
        string? webauthnChallenge = null,
        string? webauthnResponse = null
    );
    
    [MapPropertyFromSource(nameof(Guild.Icon), Use = nameof(CreateIcon))]
    [MapPropertyFromSource(nameof(Guild.Banner), Use = nameof(CreateBanner))]
    [MapPropertyFromSource(nameof(Guild.Splash), Use = nameof(CreateSplash))]
    [MapPropertyFromSource(nameof(Guild.EmbedSplash), Use = nameof(CreateEmbedSplash))]
    public partial GuildMetadata MapFromResponse(PartialGuildResponse response);

    #region Images

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Image? CreateIcon(PartialGuildResponse dto)
        => ImageFactory.MakeIcon(dto.Id, dto.Icon);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Image? CreateBanner(PartialGuildResponse dto)
        => ImageFactory.MakeBanner(dto.Id, dto.Banner, dto.BannerWidth, dto.BannerHeight);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Image? CreateSplash(PartialGuildResponse dto)
        => ImageFactory.MakeSplash(dto.Id, dto.Splash, dto.SplashWidth, dto.SplashHeight);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Image? CreateEmbedSplash(PartialGuildResponse dto)
        => ImageFactory.MakeEmbedSplash(dto.Id, dto.EmbedSplash, dto.EmbedSplashWidth, dto.EmbedSplashHeight);

    #endregion
}