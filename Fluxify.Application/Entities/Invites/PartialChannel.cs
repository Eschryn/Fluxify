using Fluxify.Application.Entities.Channels;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;

namespace Fluxify.Application.Entities.Invites;

public class PartialChannel : IChannel
{
    public Snowflake Id { get; init; }
    public string Name { get; init; }
    public ChannelType Type { get; init; }
    public PartialChannelRecipient[]? Recipients { get; set; }
    
    public string ToString(string? format, IFormatProvider? formatProvider) => Id.ToString(format, formatProvider);
    public object Clone()
    {
        return MemberwiseClone();
    }
}