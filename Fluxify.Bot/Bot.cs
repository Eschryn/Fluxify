using Fluxify.Commands;
using Fluxify.Core;
using Fluxify.Gateway;

namespace Fluxify.Bot;

public class Bot(string prefix, FluxerConfig config)
{
    public CommandCollection Commands { get; } = new();
    public GatewayClient Gateway { get; } = new(config);

    public async Task RunAsync(BotTokenCredentials credentials)
    {
        var dispatcher = Commands.BuildDispatcher(prefix, config.ServiceProvider);
        Gateway.MessageCreate += dispatcher.DispatchAsync;
        await Gateway.RunAsync(credentials);
    }
}