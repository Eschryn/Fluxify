using System.Text.Json.Serialization;

namespace Fluxify.Dto.Users;

[JsonConverter(typeof(JsonStringEnumConverter))]
enum UserThemeType
{
    Dark,
    Coal,
    Light,
    System
}