using Fluxify.Dto.Channels;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;

namespace Fluxify.Gateway.Model.Data.Guild;

public record GatewayGuildCreate(
    ChannelResponse[] Channels,
    GuildMember[] Members,
    Presence[] Presences,
    VoiceStateResponse[] VoiceStates,
    DateTimeOffset JoinedAt
);