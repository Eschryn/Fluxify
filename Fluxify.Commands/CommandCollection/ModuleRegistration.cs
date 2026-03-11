using Fluxify.Commands.Model;

namespace Fluxify.Commands.CommandCollection;

public record ModuleRegistration(ModuleMeta Meta, RegistrationEntry[] Children, string[]? Preconditions = null) : RegistrationEntry(Meta.Name, Preconditions ?? [])
{
    public ModuleMeta Meta { get; init; } = Meta;
}