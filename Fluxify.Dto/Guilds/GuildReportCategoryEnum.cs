using System.Text.Json.Serialization;

namespace Fluxify.Dto.Guilds;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GuildReportCategoryEnum
{
    Harassment,
    HateSpeech,
    ExtremistCommunity,
    IllegalActivity,
    ChildSafety,
    RaidCoordination,
    Spam,
    MalwareDistribution,
    Other
}