using Fluxify.Commands.CommandCollection;
using Fluxify.Commands.Exceptions;
using Fluxify.Commands.Model;
using Fluxify.Dto.Channels.Text.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxify.Commands.TextCommand;

public class TextCommandDispatcher
{
    private readonly string _prefix;
    private readonly IServiceProvider _serviceProvider;
    private readonly CommandTreeNode _rootTreeNode;
    private readonly List<RegistrationEntry> _registrationEntries;

    internal TextCommandDispatcher(string prefix,
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
    public async Task DispatchAsync(MessageResponse message)
    {
        if (!message.Content.StartsWith(_prefix) || message.Author.Bot == true)
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
                CheckPreconditions(commandContext, currentTreeNode);

                if (currentTreeNode.Commands.TryGetValue(command.ToString(), out var nextTreeNode))
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
                    throw new CommandNotFoundException("Command not found");
                }
            }
            catch (CommandException e)
            {
                if (e.Response is not null)
                {
                    commandContext.ReplyAsync(e.Response);
                }

                throw; // rethrow
            }

            commandContext.Tokenizer.ConsumeNext();
        }
    }

    private void CheckPreconditions(CommandContext commandContext, CommandTreeNode currentTreeNode)
    {
        foreach (var p in currentTreeNode.Preconditions)
        {
            if (commandContext.PreconditionsFulfilled.Contains(p.Name))
            {
                continue;
            }

            var result = p.Execute(commandContext);
            if (result.IsSuccess)
            {
                commandContext.PreconditionsFulfilled.Add(p.Name);
            }
            else
            {
                throw new CommandException("Precondition failed", result.Message);
            }
        }
    }
}