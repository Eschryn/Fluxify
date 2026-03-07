using Fluxify.Dto.Channels;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayGuildCreate(
    Channel[] Channels,
    GuildMember[] Members,
    Presence[] Presences,
    VoiceStateResponse[] VoiceStates,
    DateTimeOffset JoinedAt
);