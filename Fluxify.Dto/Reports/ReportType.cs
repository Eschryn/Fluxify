using System.Text.Json.Serialization;

namespace Fluxify.Dto.Reports;

[JsonConverter(typeof(JsonStringEnumConverter<ReportType>))]
public enum ReportType
{
    Guild,
    Message,
    User
}