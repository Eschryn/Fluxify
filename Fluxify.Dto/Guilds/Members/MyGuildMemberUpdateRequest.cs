using Fluxify.Core;

namespace Fluxify.Dto.Guilds.Members;

public record MyGuildMemberUpdateRequest(
    int? AccentColor,
    string? Avatar,
    string? Banner,
    string? Bio,
    Snowflake? ChannelId,
    DateTimeOffset? CommunicationsDisabledUntil,
    Snowflake? ConnectionId,
    bool? Deaf,
    bool? Mute,
    string? Nick,
    GuildMemberProfileFlags? ProfileFlags,
    string? Pronouns,
    string? TimeoutReason
);