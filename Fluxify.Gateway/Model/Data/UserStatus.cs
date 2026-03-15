using System.Text.Json.Serialization;

namespace Fluxify.Gateway.Model.Data;

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