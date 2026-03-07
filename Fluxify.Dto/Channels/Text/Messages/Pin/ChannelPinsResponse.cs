namespace Fluxify.Dto.Channels.Text.Messages.Pin;

public record ChannelPinsResponse(
    bool HasMore,
    ChannelPinResponse[] Items
);