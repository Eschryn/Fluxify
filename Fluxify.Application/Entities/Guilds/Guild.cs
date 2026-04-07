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

using System.Collections.Immutable;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Entities.Webhooks;
using Fluxify.Application.Model.Channel;
using Fluxify.Application.Repositories;
using Fluxify.Application.State;
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Settings;
using Fluxify.Rest.Guilds;

namespace Fluxify.Application.Entities.Guilds;

public class Guild(FluxerApplication app) : IEntity, ICloneable<Guild>
{
    internal RoleRepository RolesRepository
        => field ??= new RoleRepository(Id, app.Rest, new RoleMapper(), app.GuildsRepository);

    internal GuildMemberRepository MembersRepository
        => field ??=
            new GuildMemberRepository(this, app.Rest, app.UserMapper, app.UsersRepository, app.GuildsRepository,
                app.CacheConfig);

    internal GuildRequestBuilder RequestBuilder => field ??= app.Rest.Guilds[Id];

    internal PermanentCache<IGuildChannel, ChannelMapper> GuildChannels { get; } = new(app.ChannelMapper);
    internal ImmutableDictionary<Snowflake, GuildEmoji> GuildEmojis { get; set; } = ImmutableDictionary.Create<Snowflake, GuildEmoji>();
    internal ImmutableDictionary<Snowflake, Sticker> GuildStickers { get; set; } = ImmutableDictionary.Create<Snowflake, Sticker>();

    public IReadOnlyCollection<IRole> Roles => [..RolesRepository.Cache.GetAllCached().Select(r => r.Value!)];
    public IReadOnlyCollection<IGuildMember> Members => [..MembersRepository.Cache.GetAllCached().Select(x => x.Value!)];
    public IReadOnlyDictionary<Snowflake, IGuildChannel> Channels 
        => GuildChannels.GetDictionary()
            .ToImmutableDictionary(k => k.Key, v => v.Value.Value);
    public IReadOnlyCollection<GuildEmoji> Emoji => [..GuildEmojis.Values];
    public IReadOnlyCollection<Sticker> Stickers => [..GuildStickers.Values];


    internal CacheRef<IChannel>? AfkChannelRef { get;  set; }
    public GuildVoiceChannel? AfkChannel => (GuildVoiceChannel?)AfkChannelRef?.Value; 
    public int AfkTimeout { get; internal set; }
    public string? BannerHash { get; internal set; }
    public int? BannerHeight { get; internal set; }
    public int? BannerWidth { get; internal set; }
    public DefaultMessageNotifications DefaultMessageNotifications { get; internal set; }
    public GuildOperations DisabledOperations { get; internal set; }
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
    internal ICacheRef<IUser> OwnerRef { get; set; }
    public IUser Owner => OwnerRef.Value;
    public Permissions? Permissions { get; internal set; }
    internal CacheRef<IChannel>? RulesChannelRef { get; set; }
    public GuildTextChannel? RulesChannel => (GuildTextChannel?)RulesChannelRef?.Value;
    public string? SplashHash { get; internal set; }
    public SplashCardAlignment SplashCardAlignment { get; internal set; }
    public int? SplashHeight { get; internal set; }
    public int? SplashWidth { get; internal set; }
    public SystemChannelFlags SystemChannelFlags { get; internal set; }
    internal CacheRef<IChannel>? SystemChannelRef { get; set; }
    public GuildTextChannel? SystemChannel => (GuildTextChannel?)SystemChannelRef?.Value;
    public string? VanityUrlCode { get; internal set; }
    public GuildVerificationLevel VerificationLevel { get; internal set; }
    public int MemberCount { get; internal set; }

    public IUser CurrentMember =>
        field ??= MembersRepository.Cache.GetCachedOrDefault(app.CurrentUser.Id).Value!;

    public Task<GuildTextChannel> CreateTextChannelAsync(
        string name,
        Action<TextChannelProperties>? configure = null,
        CancellationToken cancellationToken = default
    ) => app.ChannelsRepository.CreateAsync<GuildTextChannel>(Id, new TextChannelProperties
    {
        Name = name
    }.Configure(configure));

    public Task<GuildVoiceChannel> CreateVoiceChannelAsync(
        string name,
        Action<VoiceChannelProperties>? configure = null
    ) => app.ChannelsRepository.CreateAsync<GuildVoiceChannel>(Id, new VoiceChannelProperties
    {
        Name = name
    }.Configure(configure));

    public Task<GuildLinkChannel> CreateLinkChannelAsync(
        string name,
        string url,
        Action<LinkChannelProperties>? configure = null
    ) => app.ChannelsRepository.CreateAsync<GuildLinkChannel>(Id, new LinkChannelProperties
    {
        Name = name,
        Url = url
    }.Configure(configure));

    public Task<GuildCategory> CreateCategoryAsync(
        string name,
        Action<CategoryProperties>? configure = null
    ) => app.ChannelsRepository.CreateAsync<GuildCategory>(Id, new CategoryProperties
    {
        Name = name
    }.Configure(configure));

    public async Task<Webhook[]> GetWebhooksAsync(
        CancellationToken cancellationToken = default
    ) => (await RequestBuilder.GetWebhooksAsync(cancellationToken))
        .Select(app.WebhookMapper.FromResponse)
        .ToArray();

    public async Task<Webhook> GetWebhookAsync(
        Snowflake id,
        CancellationToken cancellationToken = default
    ) => app.WebhookMapper.FromResponse(await app.Rest.Webhooks[id].GetAsync(cancellationToken));

    public Task<IGuildMember?> GetMemberAsync(Snowflake id) 
        => MembersRepository.GetAsync(id)
            .ContinueWith(t => t.Result.Value, TaskContinuationOptions.OnlyOnRanToCompletion);

    public Task BanAsync(
        Snowflake userId,
        int? deleteMessageDays = null,
        TimeSpan? banDuration = null,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.BanAsync(
        userId,
        deleteMessageDays,
        banDuration,
        reason,
        cancellationToken
    );

    public Task UnbanAsync(
        Snowflake userId,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.UnbanAsync(
        userId,
        reason,
        cancellationToken
    );

    public async Task<IGuildChannel?> GetChannelAsync(
        Snowflake id,
        bool bypassCache = false,
        CancellationToken cancellationToken = default
    )
    {
        var result = await app.ChannelsRepository.GetAsync(id, bypassCache);

        return result.Value as IGuildChannel;
    }

    public object Clone() => MemberwiseClone();
}