namespace Fluxify.Commands;

public record CommandRegistration(CommandMeta Meta, CommandDelegate Handler) : RegistrationEntry(Meta.Name)
{
    public CommandMeta Meta { get; init; } = Meta;
}