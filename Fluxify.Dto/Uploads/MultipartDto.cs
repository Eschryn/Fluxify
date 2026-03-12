using System.Text.Json.Serialization;

namespace Fluxify.Dto.Uploads;

public record MultipartDto([property: JsonIgnore] FileUpload[]? Files);