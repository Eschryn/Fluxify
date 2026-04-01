using System.Text.Json.Serialization;

namespace Fluxify.Dto.Instance;

[JsonConverter(typeof(JsonStringEnumConverter<KeyAlgorithm>))]
public enum KeyAlgorithm
{
    [JsonStringEnumMemberName("x25519")] X25519
}