using Fluxify.Core;

namespace Fluxify.Dto.Channels.Text.Messages;

public record MessageStickerResponse(bool Animated, Snowflake Id, string Name);