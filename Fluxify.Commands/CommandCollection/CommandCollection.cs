using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Commands.Exceptions;
using Fluxify.Commands.Model;
using Fluxify.Commands.TextCommand;

namespace Fluxify.Commands.CommandCollection;

public class CommandCollection : ICommandCollection
{
    private Func<CommandException, MessageDto?> _commandExceptionFormatter 
        = e =>
        {
            if (e is CommandNotFoundException)
                return null;
            
            return new MessageDto
            {
                Embeds =
                [
                    new Embed
                    {
                        Title = "⚠️Error",
                        Description = e.Response
                    }
                ],
                AllowedMentions = new AllowedMentions
                {
                    RepliedUser = false
                }
            };
        };
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

    public TextCommandDispatcher BuildDispatcher(string prefix, IServiceProvider serviceProvider)
    {
        return new TextCommandDispatcher(
            prefix,
            _commandExceptionFormatter,
            serviceProvider,
            CommandTreeNode.FromEntries(RegistrationEntries, Preconditions.Values.ToArray()),
            RegistrationEntries);
    }
}