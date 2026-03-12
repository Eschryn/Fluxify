using System.Net.Mime;

namespace Fluxify.Dto.Uploads;

public record ArrayFileUpload(byte[] Data, string FileName, ContentType ContentType, int SendId) 
    : FileUpload(FileName, ContentType, SendId);