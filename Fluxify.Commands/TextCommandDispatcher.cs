using Fluxify.Dto.Channels.Text.Messages;

namespace Fluxify.Commands;

public class TextCommandDispatcher
{
    private readonly string _prefix;
    private readonly CommandTreeNode _rootTreeNode;
    private readonly List<RegistrationEntry> _registrationEntries;

    private TextCommandDispatcher(string prefix, CommandTreeNode rootTreeNode, List<RegistrationEntry> registrationEntries)
    {
        _prefix = prefix;
        _rootTreeNode = rootTreeNode;
        _registrationEntries = registrationEntries;
    }

    // TODO: Replace MessageResponseSchema with own Message model
    public async Task DispatchAsync(Message message)
    {
        if (!message.Content.StartsWith(_prefix))
        {
            return;
        }

        var commandContext = new CommandContext(_prefix, message);
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
                    await cmd(commandContext);
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

    public static TextCommandDispatcher FromCommandCollection(string prefix, CommandCollection collection)
    {
        return new TextCommandDispatcher(
            prefix, CommandTreeNode.FromEntries(collection.RegistrationEntries), collection.RegistrationEntries);
    }
}