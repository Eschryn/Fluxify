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
using Fluxify.Commands.Exceptions;

namespace Fluxify.Commands;

public class CommandConfig(string prefix, IServiceProvider serviceProvider)
{
    /// <summary>
    /// This is used to reply to the user with an error message.
    /// </summary>
    public Func<CommandContext, CommandException, Task> CommandExceptionHandler { get; set; }
        = async (ctx, ex) =>
        {
            if (ex is CommandNotFoundException || ex.Response is null)
                return;

            await ctx.ReplyAsync(b => b
                .WithEmbed(e => e
                    .WithTitle("⚠️Error")
                    .WithDescription(ex.Response))
                .WithAllowedMentions(repliedUser: false));
        };

    /// <summary>
    /// The default prefix that was specified in the constructor.
    /// </summary>
    public string DefaultPrefix { get; } = prefix;

    /// <summary>
    /// The service provider that is used to resolve services.
    /// </summary>
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// Predicate used to determine if a message should be processed as a command.
    /// </summary>
    /// <remarks>Returns null if the message does not start with the prefix or if the author is a bot.</remarks>
    public Func<Message, int?> DetermineCommandStart { get; set; } =
        m => m.Author.Bot is not true && m.Content != null && m.Content.StartsWith(prefix)
            ? prefix.Length
            : null;
}