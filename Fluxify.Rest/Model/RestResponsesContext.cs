using System.Text.Json.Serialization;

namespace Fluxify.Rest.Model;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(ErrorResponse))]
public partial class RestResponsesContext : JsonSerializerContext;