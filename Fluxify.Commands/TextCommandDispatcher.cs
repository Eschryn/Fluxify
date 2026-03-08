using Fluxify.Core;
using Fluxify.Dto.Channels.Text.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxify.Commands;

public class TextCommandDispatcher
{
    private readonly string _prefix;
    private readonly IServiceProvider _serviceProvider;
    private readonly CommandTreeNode _rootTreeNode;
    private readonly List<RegistrationEntry> _registrationEntries;

    private TextCommandDispatcher(string prefix,
        IServiceProvider serviceProvider,
        CommandTreeNode rootTreeNode,
        List<RegistrationEntry> registrationEntries)
    {
        _prefix = prefix;
        _serviceProvider = serviceProvider;
        _rootTreeNode = rootTreeNode;
        _registrationEntries = registrationEntries;
    }

    // TODO: Replace MessageResponseSchema with own Message model
    public async Task DispatchAsync(Message message)
    {
        if (!message.Content.StartsWith(_prefix) && message.Author.Bot != true)
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var commandContext = new CommandContext(_prefix, message, scope.ServiceProvider);
        var currentTreeNode = _rootTreeNode;

        while (true)
        {
            var command = commandContext.Tokenizer.Peek(out _);

            try
            {
                if (currentTreeNode.Commands?.TryGetValue(command.ToString(), out var nextTreeNode) == true)
                {
                    currentTreeNode = nextTreeNode;
                }
                else if (currentTreeNode.DefaultCommand is { } cmd)
                {
                    await cmd(commandContext).ConfigureAwait(false);
                    break;
                }
                else
                {
                    throw new Exception("Command not found");
                }
            }
            finally
            {
                commandContext.Tokenizer.ConsumeNext();
            }
        }
    }

    public static TextCommandDispatcher FromCommandCollection(string prefix, CommandCollection collection, IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= new DummyProvider();
        return new TextCommandDispatcher(
            prefix, serviceProvider, CommandTreeNode.FromEntries(collection.RegistrationEntries), collection.RegistrationEntries);
    }
}