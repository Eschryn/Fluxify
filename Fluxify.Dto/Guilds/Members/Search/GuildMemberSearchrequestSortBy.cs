using System.Text.Json.Serialization;

namespace Fluxify.Dto.Guilds.Members.Search;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GuildMemberSearchrequestSortBy
{
    JoinedAt,
    Revalence
}