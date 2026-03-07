using Fluxify.Core;

namespace Fluxify.Dto.Channels.Text.Messages.BulkDelete;

public record BulkDeleteMessagesRequest(Snowflake[] MessageIds);