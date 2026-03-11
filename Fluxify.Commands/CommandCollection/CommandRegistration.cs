using Fluxify.Commands.Model;

namespace Fluxify.Commands.CommandCollection;

public record CommandRegistration(CommandMeta Meta, CommandDelegate Handler, string[]? Preconditions = null) : RegistrationEntry(Meta.Name, Preconditions ?? [])
{
    public CommandMeta Meta { get; init; } = Meta;
}