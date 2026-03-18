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

public class CommandConfiguration(string prefix, IServiceProvider serviceProvider)
{
    public Func<CommandContext, CommandException, Task> CommandExceptionHandler { get; set; }
        = async (ctx, ex) =>
        {
            if (ex is CommandNotFoundException || ex.Response is null)
                return;

            await ctx.ReplyAsync(new MessageBuilder()
                .WithEmbed(e =>
                    e.WithTitle("⚠️Error")
                        .WithDescription(ex.Response))
                .WithAllowedMentions(new AllowedMentions { RepliedUser = false })
                .Build());
        };
    
    public string DefaultPrefix { get; } = prefix;
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public Predicate<Message> IsValidCommand { get; set; } = 
        m => m.Author.Bot is not true && m.Content.StartsWith(prefix); 
}