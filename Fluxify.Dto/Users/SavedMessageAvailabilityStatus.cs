using System.Text.Json.Serialization;

namespace Fluxify.Dto.Users;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SavedMessageAvailabilityStatus
{
    Available,
    MissingPermissions,
}