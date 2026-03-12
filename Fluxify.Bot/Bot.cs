using Fluxify.Application;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Messages;
using Fluxify.Commands.CommandCollection;
using Fluxify.Commands.TextCommand;
using Fluxify.Core;
using Fluxify.Core.Types;

namespace Fluxify.Bot;

public class Bot(string prefix, FluxerConfig config) : FluxerApplication(config)
{
    public CommandCollection Commands { get; } = new();
    private TextCommandDispatcher Dispatcher { get; set; } = null!;
    
    public Task<IChannel> GetChannelAsync(Snowflake id) => Channels.GetAsync(id);

    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        Dispatcher = Commands.BuildDispatcher(prefix, Config.ServiceProvider);
        
        MessageReceived += OnMessageReceived;
        
        await base.RunAsync(cancellationToken);
    }

    private async Task OnMessageReceived(Message message)
    {
        await Dispatcher.DispatchAsync(message);
    }
}