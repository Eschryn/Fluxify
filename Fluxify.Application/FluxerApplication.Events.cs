using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Messages;
using Fluxify.Core;
using Fluxify.Dto.Channels;
using Fluxify.Gateway.Model.Data.Channel.Message;
using Fluxify.Gateway.Model.Data.Guild;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    private readonly HandlerContainer<Message> _messageHandlers = new();
    
    public event Func<Message, Task> MessageReceived
    {
        add => _messageHandlers.InsertDelegate(value);
        remove => _messageHandlers.RemoveDelegate(value);
    }
    
    private void InitializeEvents()
    {
        Gateway.MessageCreate += HandleMessageCreate;
        Gateway.ChannelCreate += HandleChannelCreate;
        Gateway.GuildCreate += HandleGuildCreate;
    }

    private async Task HandleMessageCreate(GatewayMessageCreate arg)
    {
        var message = await _messageMapper.MapAsync(arg);
        await _messageHandlers.CallHandlersAsync(message);
    }


    private Task HandleGuildCreate(GatewayGuildCreate arg)
    {
        foreach (var channelResponse in arg.Channels)
        {
            Channels.Insert(channelResponse);
        }
        
        return Task.CompletedTask;
    }

    // enrich events
    private Task HandleChannelCreate(ChannelResponse arg)
    {
        Channels.Insert(arg);
        return Task.CompletedTask;
    }
}