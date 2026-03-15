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