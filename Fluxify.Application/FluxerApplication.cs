using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Services;
using Fluxify.Application.State;
using Fluxify.Core;
using Fluxify.Gateway;
using Fluxify.Rest;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    protected readonly FluxerConfig Config;
    private readonly MessageMapper _messageMapper;
    private readonly ChannelMapper _channelMapper;
    
    protected GatewayClient Gateway { get; }
    protected RestClient Rest { get; }
    
    public FluxerApplication(FluxerConfig config)
    {
        Config = config;
        Gateway = new GatewayClient(config);
        Rest = new RestClient(config);

        _messageMapper = new MessageMapper(this);
        _channelMapper = new ChannelMapper(this);
        
        Channels = new ChannelRepository(Rest, _channelMapper);
        Messages = new MessageService(Rest, _messageMapper);
        
        InitializeEvents();
    }

    public virtual async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await Gateway.RunAsync(Config.Credentials, cancellationToken);
    } 
    
    public ChannelRepository Channels { get; }
    public MessageService Messages { get; }
}