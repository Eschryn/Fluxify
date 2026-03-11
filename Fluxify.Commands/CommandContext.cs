using Fluxify.Commands.TextCommand;
using Fluxify.Dto.Channels.Text.Messages;

namespace Fluxify.Commands;

public class CommandContext(string prefix, MessageResponse message, IServiceProvider services)
{
    public MessageResponse Message { get; } = message;
    public IServiceProvider Services { get; } = services;
    internal CommandTokenizer Tokenizer { get; } = CommandTokenizer.WithoutPrefix(prefix, message.Content);
    internal HashSet<string> PreconditionsFulfilled { get; set; } = [];

    public void ReplyAsync(string message)
    {
    }
}