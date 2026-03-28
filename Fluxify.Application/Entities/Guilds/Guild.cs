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
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Channel;
using Fluxify.Application.Repositories;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Settings;
using Fluxify.Rest.Guilds;
using UserMapper = Fluxify.Application.Entities.Users.UserMapper;

namespace Fluxify.Application.Entities.Guilds;

public class Guild(FluxerApplication app) : IEntity
{
    internal RoleRepository RolesRepository
        => field ??= new RoleRepository(Id, app.Rest, new RoleMapper(), app.Guilds);

    internal GuildMemberRepository MembersRepository
        => field ??=
            new GuildMemberRepository(this, app.Rest, new UserMapper(), app.Users, app.Guilds, app.CacheConfig);

    internal GuildRequestBuilder RequestBuilder => field ??= app.Rest.Guilds[Id];

    internal ConcurrentDictionary<Snowflake, IGuildChannel> GuildChannels { get; } = new();
    internal ConcurrentDictionary<Snowflake, GuildEmoji> GuildEmojis { get; } = new();
    internal ConcurrentDictionary<Snowflake, Sticker> GuildStickers { get; } = new();

    public IReadOnlyCollection<IRole> Roles => [..RolesRepository.Cache.GetAllCached()];
    public IReadOnlyCollection<GuildUser> Members => [..MembersRepository.Cache.GetAllCached()];
    public IReadOnlyDictionary<Snowflake, IGuildChannel> Channels => GuildChannels.AsReadOnly();
    public IReadOnlyCollection<GuildEmoji> Emoji => [..GuildEmojis.Values];
    public IReadOnlyCollection<Sticker> Stickers => [..GuildStickers.Values];

    //public GuildUser CurrentUser => MembersRepository.Cache.GetCachedOrDefault<GuildUser>(app.CurrentUser.Id)!;

    public GuildVoiceChannel? AfkChannel { get; internal set; }
    public int AfkTimeout { get; internal set; }
    public string? BannerHash { get; internal set; }
    public int? BannerHeight { get; internal set; }
    public int? BannerWidth { get; internal set; }
    public DefaultMessageNotifications DefaultMessageNotifications { get; internal set; }
    public int DisabledOperations { get; internal set; }
    public string? EmbedSplashHash { get; internal set; }
    public int? EmbedSplashHeight { get; internal set; }
    public int? EmbedSplashWidth { get; internal set; }
    public GuildExplicitContentFilter ExplicitContentFilter { get; internal set; }
    public GuildFeatureSchema[] Features { get; internal set; } = [];
    public string? IconHash { get; internal set; }
    public Snowflake Id { get; internal set; }
    public DateTimeOffset? MessageHistoryCutoff { get; internal set; }
    public GuildMfaLevel MfaLevel { get; internal set; }
    public required string Name { get; set; }
    public NsfwLevel NsfwLevel { get; internal set; }
    public required IUser Owner { get; set; }
    public Permissions? Permissions { get; internal set; }
    public GuildTextChannel? RulesChannel { get; internal set; }
    public string? SplashHash { get; internal set; }
    public SplashCardAlignment SplashCardAlignment { get; internal set; }
    public int? SplashHeight { get; internal set; }
    public int? SplashWidth { get; internal set; }
    public SystemChannelFlags SystemChannelFlags { get; internal set; }
    public GuildTextChannel? SystemChannel { get; internal set; }
    public string? VanityUrlCode { get; internal set; }
    public GuildVerificationLevel VerificationLevel { get; internal set; }

    public Task<GuildTextChannel> CreateTextChannelAsync(
        string name,
        Action<TextChannelProperties>? configure = null,
        CancellationToken cancellationToken = default
    ) => app.Channels.CreateAsync<GuildTextChannel>(Id, new TextChannelProperties
    {
        Name = name
    }.Configure(configure));

    public Task<GuildVoiceChannel> CreateVoiceChannelAsync(
        string name,
        Action<VoiceChannelProperties>? configure = null
    ) => app.Channels.CreateAsync<GuildVoiceChannel>(Id, new VoiceChannelProperties
    {
        Name = name
    }.Configure(configure));

    public Task<GuildLinkChannel> CreateLinkChannelAsync(
        string name,
        string url,
        Action<LinkChannelProperties>? configure = null
    ) => app.Channels.CreateAsync<GuildLinkChannel>(Id, new LinkChannelProperties
    {
        Name = name,
        Url = url
    }.Configure(configure));

    public Task<GuildCategory> CreateCategoryAsync(
        string name,
        Action<CategoryProperties>? configure = null
    ) => app.Channels.CreateAsync<GuildCategory>(Id, new CategoryProperties
    {
        Name = name
    }.Configure(configure));

    public Task<GuildUser> GetMemberAsync(Snowflake id) => MembersRepository.GetAsync(id);
}