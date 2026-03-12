using Fluxify.Rest.Model;

namespace Fluxify.Rest;

public class RestApiException(string code, string message, Error[] errors) : Exception(message)
{
    public string Code { get; } = code;
    public Error[] Errors { get; } = errors;
}