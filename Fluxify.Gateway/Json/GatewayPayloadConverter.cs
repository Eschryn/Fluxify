using System.Text.Json;
using System.Text.Json.Serialization;
using Fluxify.Gateway.Model;
using Fluxify.Gateway.Model.Data;

namespace Fluxify.Gateway.Json;

public class GatewayPayloadConverter : JsonConverter<GatewayPayload>
{
    public override GatewayPayload? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();
        
        GatewayOpCode opCode = default;
        string? type = null;
        int sequence = -1;
        JsonDocument? data = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
            
            var propertyName = reader.GetString();
            reader.Read();
            
            switch (propertyName)
            {
                case "op":
                    opCode = (GatewayOpCode)reader.GetInt32();
                    break;
                case "s":
                    sequence = reader.GetInt32();
                    break;
                case "t":
                    type = reader.GetString();
                    break;
                case "d":
                    data = JsonDocument.ParseValue(ref reader);
                    break;
            }
        }

        var dataType = ResolveType(opCode, type);
        return new GatewayPayload(
            opCode,
            dataType != null ? data?.Deserialize(dataType, options) : null,
            sequence,
            type);
    }

    public override void Write(Utf8JsonWriter writer, GatewayPayload value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        writer.WritePropertyName("op");
        writer.WriteNumberValue((int)value.Opcode);

        if (value.Type != null)
        {
            writer.WritePropertyName("t");
            writer.WriteStringValue(value.Type);
        }

        if (value.Data != null)
        {
            writer.WritePropertyName("d");
            JsonSerializer.Serialize(writer, value.Data, value.Data?.GetType() ?? typeof(object), options);
        }
        
        if (value.Sequence.HasValue)
        {
            writer.WritePropertyName("s");
            writer.WriteNumberValue(value.Sequence.Value);
        }
        
        writer.WriteEndObject();
    }

    private Type? ResolveType(GatewayOpCode opCode, string? s)
    {
        switch (opCode)
        {
            case GatewayOpCode.Dispatch:
                return EventNamePayloadClassMap.TypeTable.GetValueOrDefault(s ?? throw new InvalidOperationException());
            case GatewayOpCode.Hello:
                return typeof(HelloPayloadData);
            case GatewayOpCode.InvalidSession:
                return typeof(bool);
        }
        
        return null;
    }
}