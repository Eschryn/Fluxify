namespace Fluxify.Commands.Exceptions;

public class CommandNotFoundException(string? response) : CommandException("The command could not be found", response);