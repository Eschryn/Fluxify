using System.Text.Json.Serialization;
using Fluxify.Core;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Guilds.Members;

public record GuildMember(
    int? AccentColor,
    [property: JsonPropertyName("avatar")] string? AvatarHash,
    [property: JsonPropertyName("banner")] string? BannerHash,
    DateTimeOffset? CommunicationsDisabledUntil,
    bool Deaf,
    bool Mute,
    string? Nick,
    GuildMemberProfileFlags ProfileFlags,
    Snowflake[] Roles,
    User User
);