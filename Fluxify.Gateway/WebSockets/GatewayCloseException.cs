using System.Net.WebSockets;

namespace Fluxify.Gateway.WebSockets;

public class GatewayCloseException(WebSocketCloseStatus? closeStatus, string? closeStatusDescription)
    : Exception($"Gateway was closed with status {closeStatus}, reason {closeStatusDescription}")
{
    public string? CloseStatusDescription { get; } = closeStatusDescription;
    public WebSocketCloseStatus? CloseStatus { get; } = closeStatus;
}