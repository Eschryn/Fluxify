using System.Text.Json.Serialization;

namespace Fluxify.Dto.Instance;

[JsonConverter(typeof(JsonStringEnumConverter<GifProvider>))]
public enum GifProvider
{
    [JsonStringEnumMemberName("kliphy")] Kliphy,
    [JsonStringEnumMemberName("tenor")] Tenor
}