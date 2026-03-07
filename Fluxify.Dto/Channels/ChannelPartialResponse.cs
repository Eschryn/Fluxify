using Fluxify.Core;

namespace Fluxify.Dto.Channels;

public record ChannelPartialResponse(
    Snowflake Id,
    string? Name,
    ChannelPartialRecipientResponse[]? Recipients,
    ChannelType Type
);