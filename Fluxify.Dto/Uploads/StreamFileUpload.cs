using System.Net.Mime;

namespace Fluxify.Dto.Uploads;

public record StreamFileUpload(Stream Stream, string FileName, ContentType ContentType, int SendId) 
    : FileUpload(FileName, ContentType, SendId);