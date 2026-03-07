using Fluxify.Core;

namespace Fluxify.Dto.Channels.Voice;

public record StreamPreviewUploadBodySchema(
    Snowflake ChannelId,
    string? ContentType,
    string Thumbnail
);