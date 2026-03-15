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
using Fluxify.Commands.TextCommand;

namespace Fluxify.Commands;

public class CommandContext
{
    public CommandContext(string prefix, Message message, IServiceProvider services)
    {
        Message = message;
        Services = services;
        Tokenizer = CommandTokenizer.WithoutPrefix(prefix, message.Content);
        Reader = new CommandReader(Tokenizer);
    }

    public Message Message { get; }
    public IServiceProvider Services { get; }
    internal CommandTokenizer Tokenizer { get; }
    public CommandReader Reader { get; }
    internal HashSet<string> PreconditionsFulfilled { get; set; } = [];

    public Task ReplyAsync(MessageDto message) => Message.ReplyAsync(message);
    public Task ReplyAsync(string message) => Message.ReplyAsync(new MessageDto()
    {
        Content = message
    });
}