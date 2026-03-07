using System.Text.Json.Serialization;

namespace Fluxify.Gateway.Model.Data;

public record IdentifyPayloadData(
    string Token,
    Dictionary<string, string> Properties,
    List<string> IgnoredEvents, 
    PresenceUpdatePacketData Presence
);

public record PresenceUpdatePacketData(
    UserStatus Status
);

[JsonConverter(typeof(JsonStringEnumConverter<UserStatus>))]
public enum UserStatus
{
    [JsonStringEnumMemberName("online")]
    Online,
    [JsonStringEnumMemberName("idle")]
    Idle,
    [JsonStringEnumMemberName("dnd")]
    Dnd,
    [JsonStringEnumMemberName("invisible")]
    Invisible
}