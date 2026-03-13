using System.Text.Json.Serialization;
using Fluxify.Rest.RequestDtos;

namespace Fluxify.Rest.Model;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(ErrorResponse))]
[JsonSerializable(typeof(UpdateCallRegionRequest))]
[JsonSerializable(typeof(RingRequest))]
public partial class RestDtoContext : JsonSerializerContext;