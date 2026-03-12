namespace Fluxify.Dto.Uploads;

public record ArrayFileUpload(byte[] Data, string FileName, string ContentType, int SendId) 
    : FileUpload(FileName, ContentType, SendId);