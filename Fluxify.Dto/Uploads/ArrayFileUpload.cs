using Fluxify.Core.Types;

namespace Fluxify.Dto.Uploads;

public record ArrayFileUpload(byte[] Data, string FileName, string ContentType, Snowflake SendId) 
    : FileUpload(FileName, ContentType, SendId);