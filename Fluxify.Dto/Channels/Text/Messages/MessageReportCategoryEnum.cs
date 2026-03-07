using System.Text.Json.Serialization;

namespace Fluxify.Dto.Channels.Text.Messages;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MessageReportCategoryEnum
{
    Harassment,
    HateSpeech,
    ViolentContent,
    Spam,
    NsfwViolation,
    IllegalActivity,
    Doxxing,
    SelfHarm,
    ChildSafety,
    MaliciousLinks,
    Impersonation,
    Other
}