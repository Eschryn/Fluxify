using Fluxify.Dto.Channels.Text.Messages;

namespace Fluxify.Commands;

public class CommandContext(string prefix, Message message, IServiceProvider services)
{
    internal CommandTokenizer Tokenizer { get; } = CommandTokenizer.WithoutPrefix(prefix, message.Content);
    public IServiceProvider Services { get; } = services;
}