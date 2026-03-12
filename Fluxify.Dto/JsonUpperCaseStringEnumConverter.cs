using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fluxify.Dto;

/// <inheritdoc/>
public class JsonUpperCaseStringEnumConverter<T>() 
    : JsonStringEnumConverter<T>(JsonNamingPolicy.SnakeCaseUpper) where T : struct, Enum;