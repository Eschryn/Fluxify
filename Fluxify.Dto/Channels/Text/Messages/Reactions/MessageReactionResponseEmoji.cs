using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text.Messages.Reactions;

public record MessageReactionResponseEmoji(
    bool? Animated,
    Snowflake? Id,
    string Name
);