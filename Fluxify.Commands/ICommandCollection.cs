namespace Fluxify.Commands;

public interface ICommandCollection
{
    ICommandCollection Command(CommandMeta meta, CommandDelegate handler);
    ICommandCollection Module(ModuleMeta meta, Action<ICommandCollection> configure);
    TextCommandDispatcher BuildDispatcher(string prefix, IServiceProvider? serviceProvider = null);
}