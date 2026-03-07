using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fluxify.Dto;

public class StringTypeConverter<T> : JsonConverter<T> where T : StringType, new()
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        return new T()
        {
            Value = reader.GetString()!
        };
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}