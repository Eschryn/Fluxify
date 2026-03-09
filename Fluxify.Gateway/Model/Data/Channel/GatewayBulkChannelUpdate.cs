using Fluxify.Dto.Channels;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayBulkChannelUpdate(ChannelResponse[] Channels);