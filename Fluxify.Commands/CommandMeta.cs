namespace Fluxify.Commands;

public record CommandMeta(
    string Name,
    string? Description = null,
    string? Help = null,
    string[]? Aliases = null,
    string[]? Examples = null);