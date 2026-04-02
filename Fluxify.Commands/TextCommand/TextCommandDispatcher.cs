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
using Fluxify.Commands.Exceptions;
using Fluxify.Commands.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxify.Commands.TextCommand;

public class TextCommandDispatcher
{
    private readonly CommandConfig _config;
    private readonly CommandTreeNode _rootTreeNode;

    internal TextCommandDispatcher(
        CommandConfig config,
        CommandTreeNode rootTreeNode
    ) {
        _config = config;
        _rootTreeNode = rootTreeNode;
    }

    public async Task DispatchAsync(Message message)
    {
        if (_config.DetermineCommandStart(message) is not (> 0 and var start))
        {
            return;
        }

        using var scope = _config.ServiceProvider.CreateScope();
        var commandContext = new CommandContext(start, message, scope.ServiceProvider);
        var currentTreeNode = _rootTreeNode;

        while (true)
        {
            var command = commandContext.Tokenizer.Peek();

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
                    // provide info about current command
                    commandContext.Meta = currentTreeNode.Meta switch
                    {
                        CommandMeta c => c,
                        ModuleMeta m => m.DefaultCommand is { } defaultCommand ? (CommandMeta)currentTreeNode.Commands[defaultCommand].Meta : throw new InvalidOperationException(),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    
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
                _config.CommandExceptionHandler?.Invoke(commandContext, e);

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