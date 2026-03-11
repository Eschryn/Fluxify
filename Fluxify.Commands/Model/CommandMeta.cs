namespace Fluxify.Commands.Model;

public record CommandMeta(
    string Name,
    string? Description = null,
    string? Help = null,
    string[]? Aliases = null,
    string[]? Examples = null);