using System.Text.Json.Serialization;

namespace Fluxify.Rest.Model;

public record ErrorResponse(string Code, string Message, Error[]? Errors, int? RetryAfter, bool? Global);