using Fluxify.Core;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Voice;
using Fluxify.Dto.SavedMedia;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.GuildSettings;
using Fluxify.Dto.Users.Relationships;
using Fluxify.Dto.Users.Settings;
using Fluxify.Gateway.Model.Dto;

namespace Fluxify.Gateway.Model.Data;

/// <summary>
/// Sent by the server after successful identification
/// </summary>
/// <param name="Version">The gateway protocol version</param>
/// <param name="SessionId">Identifier for the current session</param>
public record ReadyPayload(
    int Version,
    string SessionId,
    UserPrivate User,
    GuildReadyData[] Guilds,
    ChannelResponse[] PrivateChannels,
    Relationship[] Relationships,
    UserResponse[] Users,
    Presence[] Presences,
    GatewaySession[] Sessions,
    UserSettings? UserSettings,
    UserGuildSettings[]? UserGuildSettings,
    ReadState[]? ReadStates,
    Dictionary<string, string>? Notes,
    string? CountryCode,
    Snowflake[]? PinnedDms,
    FavoriteMemeResponse[]? FavoriteMemes,
    string? AuthSessionIdHash,
    RtcRegion[]? RtcRegions
);