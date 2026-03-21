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

using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Repositories;
using Fluxify.Core;
using Fluxify.Gateway;
using Fluxify.Gateway.Model.Data.Guild.Roles;
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

    public GatewayClient Gateway { get; }
    public RestClient Rest { get; }
    
    public FluxerApplication(FluxerConfig config, GatewayConfig? gatewayConfig = null)
    {
        Config = config;
        Gateway = new GatewayClient(config, gatewayConfig);
        Rest = new RestClient(config);

        MessageMapper = new MessageMapper(this);
        _channelMapper = new ChannelMapper(this);
        _userMapper = new UserMapper();
        _guildMapper = new GuildMapper(this);
        
        Channels = new ChannelRepository(Rest, _channelMapper);
        Users = new UserRepository(Rest, _userMapper);
        Guilds = new GuildRepository(Rest, _guildMapper);
        
        InitializeEvents();
    }

    public virtual async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var gatewayBotResponse = await Rest.Gateway.GetGatewayBotAsync(cancellationToken);
        if (gatewayBotResponse == null)
        {
            throw new Exception("Could not get gateway information.");
        }
        
        await Gateway.RunAsync(new Uri(gatewayBotResponse.Url), cancellationToken);
    } 
    
    public ChannelRepository Channels { get; }
    public UserRepository Users { get; }
    public GuildRepository Guilds { get; }
}