using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fluxify.Dto;

/// <inheritdoc/>
class JsonUpperCaseStringEnumConverter<T>() 
    : JsonStringEnumConverter<T>(JsonNamingPolicy.SnakeCaseUpper) where T : struct, Enum;