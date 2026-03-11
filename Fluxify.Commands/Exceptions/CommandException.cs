namespace Fluxify.Commands.Exceptions;

public class CommandException(
    string message,
    string? response
) : Exception(message)
{
    public string? Response { get; } = response;
}