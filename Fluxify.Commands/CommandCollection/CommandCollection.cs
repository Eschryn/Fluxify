using Fluxify.Commands.Model;
using Fluxify.Commands.TextCommand;

namespace Fluxify.Commands.CommandCollection;

public class CommandCollection : ICommandCollection
{
    private List<RegistrationEntry> RegistrationEntries { get; set; } = [];
    private Dictionary<string, Precondition> Preconditions { get; set; } = [];

    public ICommandCollection Precondition(Precondition precondition)
    {
        Preconditions.TryAdd(precondition.Name, precondition);
        return this;
    }
    
    public ICommandCollection Module(ModuleMeta meta, Action<ICommandCollection> configure, string[]? preconditions)
    {
        var container = new CommandCollection();
        configure(container);
        RegistrationEntries.Add(new ModuleRegistration(meta, container.RegistrationEntries.ToArray(), preconditions));
        return this;
    }

    public ICommandCollection Command(CommandMeta meta, CommandDelegate handler, string[]? preconditions)
    {
        RegistrationEntries.Add(new CommandRegistration(meta, handler, preconditions));
        return this;
    }

    public TextCommandDispatcher BuildDispatcher(string prefix, IServiceProvider? serviceProvider = null)
    {
        return new TextCommandDispatcher(
            prefix,
            serviceProvider,
            CommandTreeNode.FromEntries(RegistrationEntries, Preconditions.Values.ToArray()),
            RegistrationEntries);
    }
}