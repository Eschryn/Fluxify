using Fluxify.Core.Types;

namespace Fluxify.Dto.Uploads;

public abstract record FileUpload(
    string FileName,
    string ContentType,
    Snowflake SendId);