using Fluxify.Dto.Channels;

namespace Fluxify.Gateway.Model.Data.Channel;

public record GatewayBulkChannelUpdate(ChannelResponse[] Channels);