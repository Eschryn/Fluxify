using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;

namespace Fluxify.Application.Entities.Channels;

public static class TextChannelExtensions {
    extension(TextChannel channel)
    {
        public Task<Message?> SendMessageAsync(string message) 
        {
            return channel.SendMessageAsync(new MessageDto
            {
                Content = message
            });
        }
    }
}