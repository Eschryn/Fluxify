using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages;

namespace Fluxify.Dto.Users;

public record SavedMessageEntryResponse(
    Snowflake ChannelId,
    Snowflake Id,
    MessageResponse? Message,
    Snowflake MessageId,
    SavedMessageAvailabilityStatus Status
);