using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fluxify.Dto.Common;

public class MediaHashConverter : JsonConverter<MediaHash>
{
    public override MediaHash Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Expected string for MediaHash, but got " + reader.TokenType);
        
        return new MediaHash(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, MediaHash value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Hash);
    }
}