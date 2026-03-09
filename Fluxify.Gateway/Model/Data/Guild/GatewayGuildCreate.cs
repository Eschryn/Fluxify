using Fluxify.Dto.Channels;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayGuildCreate(
    ChannelResponse[] Channels,
    GuildMember[] Members,
    Presence[] Presences,
    VoiceStateResponse[] VoiceStates,
    DateTimeOffset JoinedAt
);