namespace Fluxify.Gateway;

public enum ConnectionState
{
    Reconnecting = -1,
    Connecting = 0,
    Authenticating = 1,
    Connected = 2,
    Disconnected = 3,
}