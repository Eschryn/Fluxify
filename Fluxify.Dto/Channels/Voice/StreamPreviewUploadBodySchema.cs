using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Voice;

public record StreamPreviewUploadBodySchema(
    Snowflake ChannelId,
    string? ContentType,
    string Thumbnail
);