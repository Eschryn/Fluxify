using System.Text.Json.Serialization;

namespace Fluxify.Dto.Users;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserReportCategoryEnum
{
    Harassment,
    HateSpeech,
    SpamAccount,
    Impersonation,
    UnderageUser,
    InappropriateProfile,
    Other
}