using Fluxify.Core;

namespace Fluxify.Dto.Users;

public record PreloadMessagesRequest(Snowflake[] Channels);