using System.Text.Json.Serialization;
using Fluxify.Gateway.Json;

namespace Fluxify.Gateway.Model;

[JsonConverter(typeof(GatewayPayloadConverter))]
public record GatewayPayload(
    GatewayOpCode Opcode,
    object? Data,
    int? Sequence = null,
    string? Type = null);