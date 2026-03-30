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

using Fluxify.Application;
using Fluxify.Application.EventArgs;
using Fluxify.Commands.CommandCollection;
using Fluxify.Commands.TextCommand;
using Fluxify.Core;
using Fluxify.Gateway;

namespace Fluxify.Bot;

public class Bot(BotConfig config) : FluxerApplication(config)
{
    [Obsolete("Use Bot(BotConfig config) instead", false)]
    public Bot(string prefix, FluxerConfig config, GatewayConfig? gatewayConfig = null) 
        : this(new BotConfig(prefix)
        {
            FluxerConfig =
            {
                CredentialProvider = config.CredentialProvider,
                LoggerFactory = config.LoggerFactory,
                ServiceProvider = config.ServiceProvider,
                Credentials = config.Credentials,
                HttpClientFactory = config.HttpClientFactory,
                InstanceUri = config.InstanceUri
            },
            GatewayConfig = gatewayConfig ?? new GatewayConfig()
        }) {}
    
    public CommandCollection Commands { get; } = new();
    private TextCommandDispatcher Dispatcher { get; set; } = null!;

    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        Dispatcher = Commands.BuildDispatcher(config.CommandConfig);
        
        MessageReceived += OnMessageReceived;
        
        await base.RunAsync(cancellationToken);
    }

    private Task OnMessageReceived(MessageEventArgs args) => Dispatcher.DispatchAsync(args.Message);
}