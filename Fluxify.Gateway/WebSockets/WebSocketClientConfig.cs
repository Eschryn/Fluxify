using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fluxify.Gateway.WebSockets;

public class WebSocketClientConfig
{
    public int ReceiveBufferSize { get; set; } = 16 * 1024;
    public ILogger? Logger { get; set; } = NullLogger.Instance;
}