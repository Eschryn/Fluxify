using Fluxify.Commands.Model;

namespace Fluxify.Commands.CommandCollection;

public static class CommandCollectionExtensions
{
    extension(ICommandCollection collection)
    {
        public ICommandCollection Module(string name, Action<ICommandCollection> func, string[]? preconditions)
        {
            return collection.Module(new ModuleMeta(name, string.Empty, string.Empty), func, preconditions);
        }

        public ICommandCollection Module(string name, Action<ICommandCollection> func, params Precondition[]? preconditions)
        {
            return collection
                .AddPreconditions(preconditions)
                .Module(new ModuleMeta(name, string.Empty, string.Empty), func, preconditions?.Select(p => p.Name)?.ToArray());
        }

        public ICommandCollection Command(CommandMeta meta, Delegate handler, string[]? preconditions = null)
            => collection.Command(meta, CommandDelegateFactory.Create(handler), preconditions);

        public ICommandCollection Command(string name, Delegate handler, string[]? preconditions)
            => collection.Command(new CommandMeta(name, string.Empty, string.Empty), handler, preconditions);
        public ICommandCollection Command(string name, Delegate handler, params Precondition[]? preconditions)
        {
            return collection
                .AddPreconditions(preconditions)
                .Command(new CommandMeta(name, string.Empty, string.Empty), handler,
                preconditions?.Select(p => p.Name)?.ToArray());
        }

        private ICommandCollection AddPreconditions(Precondition[]? preconditions)
        {
            if (preconditions == null)
            {
                return collection;
            }

            foreach (var precondition in preconditions)
            {
                collection.Precondition(precondition);
            }
            
            return collection;
        }
    }
}