namespace Fluxify.Dto.Channels.Text.Messages.Pin;

public record ChannelPinResponse(
    ChannelPinMessageResponse Message,
    DateTimeOffset PinnedAt
);