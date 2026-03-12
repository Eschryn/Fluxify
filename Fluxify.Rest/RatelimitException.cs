namespace Fluxify.Rest;

public class RatelimitException(string code, string message, int retryAfter, bool global) : Exception(message)
{
    public string Code { get; } = code;
    public int RetryAfter { get; } = retryAfter;
    public bool Global { get; } = global;
}