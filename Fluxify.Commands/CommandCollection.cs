namespace Fluxify.Commands;

public class CommandCollection : ICommandCollection
{
    public List<RegistrationEntry> RegistrationEntries { get; set; } = [];

    public ICommandCollection Module(ModuleMeta meta, Action<ICommandCollection> configure)
    {
        var container = new CommandCollection();
        configure(container);
        RegistrationEntries.Add(new ModuleRegistration(meta, container.RegistrationEntries.ToArray()));
        return this;
    }

    public ICommandCollection Command(CommandMeta meta, CommandDelegate handler)
    {
        RegistrationEntries.Add(new CommandRegistration(meta, handler));
        return this;
    }

    public TextCommandDispatcher BuildDispatcher(string prefix, IServiceProvider? serviceProvider = null) => TextCommandDispatcher.FromCommandCollection(prefix, this, serviceProvider);
}