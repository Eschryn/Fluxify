namespace Fluxify.Commands;

public record ModuleRegistration(ModuleMeta Meta, RegistrationEntry[] Children) : RegistrationEntry(Meta.Name)
{
    public ModuleMeta Meta { get; init; } = Meta;
}