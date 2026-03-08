using System.Text.Json.Serialization;

namespace Fluxify.Dto.Common;

[JsonConverter(typeof(JsonStringEnumConverter<ConnectionType>))]
public enum ConnectionType
{
    Bsky,
    Domain 
}