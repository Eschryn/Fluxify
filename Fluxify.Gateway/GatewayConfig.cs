using Fluxify.Gateway.WebSockets;

namespace Fluxify.Gateway;

public class GatewayConfig
{
    public WebSocketClientConfig WebSocketClientConfig { get; set; } = new();
    public TimeSpan SendTimeout { get; set; } = TimeSpan.FromMinutes(30);

    public string[] IgnoredGatewayEvents { get; set; } = [];
    
    // decide if we maybe want to restore a session from a file
    // public string SessionFile { get; set; } = "last_session";
    
    internal Dictionary<string, string> DeviceProperties { get; } = new()
    {
        ["os"] = Environment.OSVersion.Platform.ToString(),
        ["browser"] = "Fluxify.Bot",
        ["device"] = "Fluxify.Bot"
    };
}