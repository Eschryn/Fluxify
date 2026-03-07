using Fluxify.Core;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Gateway.Model.Dto;

public record VoiceStateResponse(
    Snowflake? GuildId,
    Snowflake? ChannelId,
    Snowflake UserId,
    string? ConnectionId,
    string? SessionId,
    GuildMember? Member,
    bool Mute,
    bool Deaf,
    bool SelfMute,
    bool SelfDeaf,
    bool? SelfVideo,
    bool? SelfStream,
    bool? IsMobile,
    string[] ViewerStreamKeys,
    int? Version
);