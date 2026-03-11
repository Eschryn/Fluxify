using Fluxify.Commands.Model;
using Fluxify.Commands.TextCommand;

namespace Fluxify.Commands.CommandCollection;

public interface ICommandCollection
{
    ICommandCollection Command(CommandMeta meta, CommandDelegate handler, string[]? preconditions = null);
    ICommandCollection Module(ModuleMeta meta, Action<ICommandCollection> configure, string[]? preconditions = null);
    TextCommandDispatcher BuildDispatcher(string prefix, IServiceProvider? serviceProvider = null);
    ICommandCollection Precondition(Precondition precondition);
}