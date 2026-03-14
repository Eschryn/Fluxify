using Fluxify.Core.Types;

namespace Fluxify.Dto.Uploads;

public record StreamFileUpload(Stream Stream, string FileName, string ContentType, Snowflake SendId) 
    : FileUpload(FileName, ContentType, SendId);