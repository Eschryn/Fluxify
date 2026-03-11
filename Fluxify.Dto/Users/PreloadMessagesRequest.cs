using Fluxify.Core.Types;

namespace Fluxify.Dto.Users;

public record PreloadMessagesRequest(Snowflake[] Channels);