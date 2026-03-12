using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;
using Fluxify.Commands.TextCommand;

namespace Fluxify.Commands;

public class CommandContext(string prefix, Message message, IServiceProvider services)
{
    public Message Message { get; } = message;
    public IServiceProvider Services { get; } = services;
    internal CommandTokenizer Tokenizer { get; } = CommandTokenizer.WithoutPrefix(prefix, message.Content);
    internal HashSet<string> PreconditionsFulfilled { get; set; } = [];

    public Task ReplyAsync(MessageDto message) => Message.ReplyAsync(message);
    public Task ReplyAsync(string message) => Message.ReplyAsync(new MessageDto()
    {
        Content = message
    });
}