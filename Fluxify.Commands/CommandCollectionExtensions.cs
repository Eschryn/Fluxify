namespace Fluxify.Commands;

public static class CommandCollectionExtensions
{
    extension(ICommandCollection collection)
    {
        public ICommandCollection Module(string name, Action<ICommandCollection> func)
        {
            return collection.Module(new ModuleMeta(name, string.Empty, string.Empty), func);
        }

        public ICommandCollection Command(CommandMeta meta, Delegate handler)
            => collection.Command(meta, CommandDelegateFactory.Create(handler));

        public ICommandCollection Command(string name, Delegate handler)
            => collection.Command(new CommandMeta(name, string.Empty, string.Empty), handler);
    }
}