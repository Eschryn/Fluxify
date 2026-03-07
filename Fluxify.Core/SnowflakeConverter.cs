using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fluxify.Core;

public class SnowflakeConverter : JsonConverter<Snowflake>
{
    public override Snowflake Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (ulong.TryParse(reader.GetString(), out var result))
            {
                return new Snowflake(result);
            }
            
            throw new JsonException();
        } else if (reader.TokenType == JsonTokenType.Number)
            return new Snowflake(reader.GetUInt64());
        
        throw new JsonException();       
    }

    public override void Write(Utf8JsonWriter writer, Snowflake value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(((ulong)value).ToString());
    }
}