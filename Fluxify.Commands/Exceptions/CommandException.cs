namespace Fluxify.Commands.Exceptions;

public class CommandException(
    string response
) : Exception(response)
{
    public string? Response { get; } = response;
}