namespace Fluxify.Gateway.Model.Data;

public record IdentifyPayloadData(
    string Token,
    Dictionary<string, string> Properties,
    string[] IgnoredEvents, 
    PresenceUpdate Presence
);