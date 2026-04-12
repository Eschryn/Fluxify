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

using Fluxify.Application.Common;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Invites;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Entities.Webhooks;
using Fluxify.Application.Repositories;
using Fluxify.Core.Credentials;
using Fluxify.Dto.Instance;
using Fluxify.Dto.Users;
using Fluxify.Gateway;
using Fluxify.Rest;
using MemberMapper = Fluxify.Application.Entities.Guilds.Members.MemberMapper;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    protected readonly ApplicationConfig Config;
    internal readonly MessageMapper MessageMapper;
    internal readonly ChannelMapper ChannelMapper;
    internal readonly UserMapper UserMapper;
    internal readonly InviteMapper InviteMapper;
    internal readonly GuildMapper GuildMapper;
    internal readonly CacheConfig CacheConfig = new();
    internal readonly WebhookMapper WebhookMapper;
    internal readonly ImageFactory ImageFactory;
    internal readonly CacheMapper CacheMapper;
    internal readonly MemberMapper MemberMapper;

    public GatewayClient Gateway { get; }
    public RestClient Rest { get; }

    private ICacheRef<PrivateUser>? CurrentUserRef { get; set; }
    public PrivateUser CurrentUser => CurrentUserRef?.Value 
                                      ?? throw new InvalidOperationException("Clients needs to be logged in.");

    internal WellKnownFluxerResponse? InstanceInfo { get; private set; }

    public FluxerApplication(ApplicationConfig config)
    {
        Config = config;
        Gateway = new GatewayClient(config.FluxerConfig, config.GatewayConfig);
        Rest = new RestClient(config.FluxerConfig);

        CacheMapper = new CacheMapper(this);
        ImageFactory = new ImageFactory(this);
        
        WebhookMapper = new WebhookMapper(this);
        MessageMapper = new MessageMapper(this);
        InviteMapper = new InviteMapper(this);
        ChannelMapper = new ChannelMapper(this);
        UserMapper = new UserMapper(this);
        GuildMapper = new GuildMapper(this);
        MemberMapper = new MemberMapper(this);


        ChannelsRepository = new ChannelRepository(Rest, ChannelMapper, CacheConfig);
        UsersRepository = new UserRepository(Rest, UserMapper, CacheConfig);
        GuildsRepository = new GuildRepository(Rest, GuildMapper, CacheConfig);

        InitializeEvents();
    }

    public virtual async Task RunAsync(CancellationToken cancellationToken = default)
    {
        InstanceInfo = await Rest.GetWellKnownAsync();

        Uri gatewayUri;
        var credentials = await Config.FluxerConfig.CredentialProvider();
        if (credentials is BotTokenCredentials)
        {
            var gatewayBotResponse = await Rest.Gateway.GetGatewayBotAsync(cancellationToken);
            if (gatewayBotResponse == null)
            {
                throw new Exception("Could not get gateway information.");
            }

            gatewayUri = new Uri(gatewayBotResponse.Url);
        }
        else
        {
            gatewayUri = InstanceInfo!.Endpoints.Gateway;
        }

        await Gateway.RunAsync(gatewayUri, cancellationToken);
    }

    internal ChannelRepository ChannelsRepository { get; }
    internal UserRepository UsersRepository { get; }
    internal GuildRepository GuildsRepository { get; }

    public IReadOnlyCollection<CacheRef<Guild>> Guilds => GuildsRepository.Cache.GetAllCached();

    public IReadOnlyCollection<PrivateTextChannel> PrivateChannels
        => ChannelsRepository.Cache.GetAllCached().Select(c => c.Value).OfType<PrivateTextChannel>().ToArray();


    public Task<Dm> GetOrCreateDmAsync(Snowflake userId,
        CancellationToken cancellationToken = default)
        => ChannelsRepository.CreateOrGetPrivateChannelAsync<Dm>(
            new CreatePrivateChannelRequest(RecipientId: userId, Recipients: null), cancellationToken);

    public async Task<Guild> GetGuildAsync(
        Snowflake guildId,
        bool bypassCache = false
    ) => (await GuildsRepository.GetAsync(guildId, bypassCache)).Value!;

    public async Task<IChannel> GetChannelAsync(
        Snowflake channelId,
        bool bypassCache = false
    ) => (await ChannelsRepository.GetAsync(channelId, bypassCache)).Value!;

    public async Task<GlobalUser> GetUserAsync(
        Snowflake userId,
        bool bypassCache = false
    ) => (await UsersRepository.GetAsync(userId, bypassCache)).Value!;
}