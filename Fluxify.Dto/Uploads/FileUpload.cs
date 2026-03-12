using System.Net.Mime;

namespace Fluxify.Dto.Uploads;

public abstract record FileUpload(
    string FileName,
    ContentType ContentType,
    int SendId);