using System.Text.Json.Serialization;

namespace Fluxify.Dto.Packs;

[JsonConverter(typeof(JsonNumberEnumConverter<PackType>))]
public enum PackType
{
    Emoji,
    Sticker
}