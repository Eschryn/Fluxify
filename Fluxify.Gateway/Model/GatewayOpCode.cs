namespace Fluxify.Gateway.Model;

public enum GatewayOpCode
{
    /// <summary>
    /// Send by the server together with event data and an event type to notify the client about changes
    /// </summary>
    Dispatch = 0,
    /// <summary>
    /// Sent by the server when it wants you to send another heartbeat.
    /// Client sends it to show the server that it's still alive.
    /// </summary>
    Heartbeat = 1,
    /// <summary>
    /// Sent by the client to authenticate in the gateway
    /// </summary>
    Identify = 2,
    PresenceUpdate = 3,
    VoiceStateUpdate = 4,
    VoiceServerPing = 5,
    /// <summary>
    /// Send by the client to request to restore the previous gateway session
    /// </summary>
    Resume = 6,
    /// <summary>
    /// Send by the server to tell the client to reconnect - compared to InvalidSession this does not invalidate the current session.
    /// </summary>
    Reconnect = 7,
    RequestGuildMembers = 8,
    /// <summary>
    /// Sent by the server to indicate that the gateway session has been invalidated and the client needs to reconnect.
    /// </summary>
    InvalidSession = 9,
    /// <summary>
    /// Sent by the server to indicate that the gateway session is initialized and ready to process payloads.
    /// The next step is to authenticate or restore a session.
    /// </summary>
    Hello = 10,
    /// <summary>
    /// Send by the server when the server acknowledges your last heartbeat.
    /// </summary>
    HeartbeatAck = 11,
    GatewayError = 12,
    LazyRequest = 14
}