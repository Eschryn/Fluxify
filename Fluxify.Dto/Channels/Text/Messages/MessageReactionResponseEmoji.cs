using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text.Messages;

public record MessageReactionResponseEmoji(
    bool? Animated,
    Snowflake? Id,
    string Name
);