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
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Repositories;
using Fluxify.Core.Credentials;
using Fluxify.Core.Types;
using Fluxify.Gateway;
using Fluxify.Rest;
using UserMapper = Fluxify.Application.Entities.Users.UserMapper;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    protected readonly FluxerConfig Config;
    internal readonly MessageMapper MessageMapper;
    private readonly ChannelMapper _channelMapper;
    private readonly UserMapper _userMapper;
    private readonly GuildMapper _guildMapper;
    internal readonly CacheConfig CacheConfig = new();

    public GatewayClient Gateway { get; }
    public RestClient Rest { get; }
    
    public PrivateUser CurrentUser { get; private set; }
    
    public FluxerApplication(FluxerConfig config, GatewayConfig? gatewayConfig = null)
    {
        Config = config;
        Gateway = new GatewayClient(config, gatewayConfig);
        Rest = new RestClient(config);

        MessageMapper = new MessageMapper(this);
        _channelMapper = new ChannelMapper(this);
        _userMapper = new UserMapper();
        _guildMapper = new GuildMapper(this);

        ChannelsRepository = new ChannelRepository(Rest, _channelMapper, CacheConfig);
        UsersRepository = new UserRepository(Rest, _userMapper, CacheConfig);
        GuildsRepository = new GuildRepository(Rest, _guildMapper, CacheConfig);

        InitializeEvents();
    }

    public virtual async Task RunAsync(CancellationToken cancellationToken = default)
    {
        Uri gatewayUri;
        var credentials = await Config.CredentialProvider();
        if (credentials is BotTokenCredentials botTokenCredentials)
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
            // TODO: get from instance data
            gatewayUri = new Uri("wss://gateway.fluxer.app/");
        }
        
        await Gateway.RunAsync(gatewayUri, cancellationToken);
    }

    internal ChannelRepository ChannelsRepository { get; }
    internal UserRepository UsersRepository { get; }
    internal GuildRepository GuildsRepository { get; }

    public IReadOnlyCollection<Guild> Guilds => GuildsRepository.Cache.GetAllCached();

    public IReadOnlyCollection<PrivateTextChannel> PrivateChannels
        => ChannelsRepository.Cache.GetAllCached().OfType<PrivateTextChannel>().ToArray();

    public Task<Guild> GetGuildAsync(
        Snowflake guildId,
        bool bypassCache = false
    ) => GuildsRepository.GetAsync(guildId, bypassCache);

    public Task<IChannel> GetChannelAsync(
        Snowflake channelId,
        bool bypassCache = false
    ) => ChannelsRepository.GetAsync(channelId, bypassCache);
    
    public Task<GlobalUser> GetUserAsync(
        Snowflake userId,
        bool bypassCache = false
    ) => UsersRepository.GetAsync(userId, bypassCache);
}