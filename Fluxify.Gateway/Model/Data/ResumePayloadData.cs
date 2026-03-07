using System.Text.Json.Serialization;

namespace Fluxify.Gateway.Model.Data;

public record ResumePayloadData(
    string Token,
    string SessionId,
    [property: JsonPropertyName("seq")] 
    int? LastSequence);