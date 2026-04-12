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
using System.Collections.Immutable;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Guild;
using Fluxify.Application.Repositories;
using Fluxify.Application.State;
using Fluxify.Dto.Guilds.Settings;
using Fluxify.Rest.Guilds;

namespace Fluxify.Application.Entities.Guilds;

public partial class Guild : GuildMetadata, IEntity, ICloneable<Guild>
{
    private readonly FluxerApplication _app;

    internal RoleRepository RolesRepository
        => field ??= new RoleRepository(Id, _app.Rest, new RoleMapper(), _app.GuildsRepository);

    internal GuildMemberRepository MembersRepository
        => field ??=
            new GuildMemberRepository(this, _app.Rest, _app.MemberMapper, _app.UsersRepository, _app.GuildsRepository,
                _app.CacheConfig);

    internal GuildRequestBuilder RequestBuilder => field ??= _app.Rest.Guilds[Id];

    internal ConcurrentDictionary<Snowflake, ICacheRef<IGuildChannel>> GuildChannels { get; } = new();

    [MapperIgnore]
    internal ImmutableDictionary<Snowflake, GuildEmoji> GuildEmojis { get; set; } =
        ImmutableDictionary.Create<Snowflake, GuildEmoji>();

    [MapperIgnore]
    internal ImmutableDictionary<Snowflake, Sticker> GuildStickers { get; set; } =
        ImmutableDictionary.Create<Snowflake, Sticker>();

    public ICacheRef<IUser> Owner { get; }

    internal CacheRef<IChannel>? AfkChannelRef { get; set; }

    internal CacheRef<IChannel>? RulesChannelRef { get; set; }

    internal CacheRef<IChannel>? SystemChannelRef { get; set; }


    public IReadOnlyCollection<IRole> Roles => [..RolesRepository.Cache.GetAllCached().Select(r => r.Value!)];

    public IReadOnlyCollection<IGuildMember> Members =>
        [..MembersRepository.Cache.GetAllCached().Select(x => x.Value!)];

    public IReadOnlyDictionary<Snowflake, IGuildChannel> Channels
        => GuildChannels
            .Where(x => x.Value.Value is not null)
            .ToImmutableDictionary(k => k.Key, v => v.Value.Value)!;

    public IReadOnlyCollection<GuildEmoji> Emoji => [..GuildEmojis.Values];
    public IReadOnlyCollection<Sticker> Stickers => [..GuildStickers.Values];
    public GuildVoiceChannel? AfkChannel => (GuildVoiceChannel?)AfkChannelRef?.Value;
    public GuildTextChannel? RulesChannel => (GuildTextChannel?)RulesChannelRef?.Value;
    public GuildTextChannel? SystemChannel => (GuildTextChannel?)SystemChannelRef?.Value;
    public int MemberCount { get; internal set; }
    public string? VanityUrlCode { get; internal set; }
    public int AfkTimeout { get; internal set; }
    public GuildVerificationLevel VerificationLevel { get; internal set; }
    public DefaultMessageNotifications DefaultMessageNotifications { get; internal set; }
    public GuildExplicitContentFilter ExplicitContentFilter { get; internal set; }
    public GuildOperations DisabledOperations { get; internal set; }
    public DateTimeOffset? MessageHistoryCutoff { get; internal set; }
    public GuildMfaLevel MfaLevel { get; internal set; }
    public NsfwLevel NsfwLevel { get; internal set; }
    public Permissions? Permissions { get; internal set; }
    public SystemChannelFlags SystemChannelFlags { get; internal set; }

    public IUser CurrentMember =>
        field ??= MembersRepository.Cache.GetCachedOrDefault(_app.CurrentUser.Id).Value!;

    internal Guild(FluxerApplication app,
        ICacheRef<IUser> owner,
        string name,
        GuildFeatureSchema[] features
    ) : base(name, features)
    {
        _app = app;
        Owner = owner;
    }

    public object Clone() => MemberwiseClone();
}