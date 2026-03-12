namespace Fluxify.Application.Entities.Messages;

public enum MessageType
{
    Regular = 0,
    UserAddMessage = 1,
    UserRemoveMessage = 2,
    CallMessage = 3,
    ChannelNameChangeMessage = 4,
    ChannelIconChangeMessage = 5,
    MessagePinnedMessage = 6,
    UserJoinMessage = 7,
    ReplyMessage = 19
}