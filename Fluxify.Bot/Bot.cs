using Fluxify.Commands;
using Fluxify.Commands.CommandCollection;
using Fluxify.Core;
using Fluxify.Gateway;

namespace Fluxify.Bot;

public class Bot
{
    private readonly string _prefix;
    private readonly FluxerConfig _config;

    public Bot(string prefix, FluxerConfig config)
    {
        _prefix = prefix;
        _config = config;
        Gateway = new GatewayClient(config);
    }

    public CommandCollection Commands { get; } = new();
    public GatewayClient Gateway { get; }

    public async Task RunAsync()
    {
        var dispatcher = Commands.BuildDispatcher(prefix, config.ServiceProvider);
        Gateway.MessageCreate += dispatcher.DispatchAsync;
        await Gateway.RunAsync(_config.Credentials);
    }
}