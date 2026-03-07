using System.Text.Json.Serialization;

namespace Fluxify.Dto.Users.DataHarvest;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HarvestStatusEnum
{
    Pending,
    Processing,
    Completed,
    Failed,
}