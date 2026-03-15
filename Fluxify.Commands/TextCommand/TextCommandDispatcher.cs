// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.Entities.Messages;
using Fluxify.Commands.CommandCollection;
using Fluxify.Commands.Exceptions;
using Fluxify.Commands.Model;
using Fluxify.Dto.Channels.Text.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxify.Commands.TextCommand;

public class TextCommandDispatcher
{
    private readonly string _prefix;
    private readonly Func<CommandException, MessageDto?> _commandExceptionFormatter;
    private readonly IServiceProvider _serviceProvider;
    private readonly CommandTreeNode _rootTreeNode;
    private readonly List<RegistrationEntry> _registrationEntries;

    internal TextCommandDispatcher(
        string prefix,
        Func<CommandException, MessageDto?> commandExceptionFormatter,
        IServiceProvider serviceProvider,
        CommandTreeNode rootTreeNode,
        List<RegistrationEntry> registrationEntries)
    {
        _prefix = prefix;
        _commandExceptionFormatter = commandExceptionFormatter;
        _serviceProvider = serviceProvider;
        _rootTreeNode = rootTreeNode;
        _registrationEntries = registrationEntries;
    }

    public async Task DispatchAsync(Message message)
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
                    commandContext.Tokenizer.ConsumeNext();
                    currentTreeNode = nextTreeNode;
                }
                else if (currentTreeNode.DefaultCommand is { } cmd)
                {
                    await cmd(commandContext).ConfigureAwait(false);
                    break;
                }
                else
                {
                    throw new CommandNotFoundException();
                }
            }
            catch (CommandException e)
            {
                if (e.Response is not null && _commandExceptionFormatter(e) is {} messageDto)
                {
                    await commandContext.ReplyAsync(messageDto);
                }

                break;
            }
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
                throw new CommandException(result.Message ?? "Precondition failed");
            }
        }
    }
}