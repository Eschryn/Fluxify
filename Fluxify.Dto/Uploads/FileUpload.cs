using System.Net.Mime;

namespace Fluxify.Dto.Uploads;

public abstract record FileUpload(
    string FileName,
    string ContentType,
    int SendId);