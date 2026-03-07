using System.Text.Json.Serialization;
using Fluxify.Core;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Gateway.Json;

namespace Fluxify.Gateway.Model.Data.Channel;

public record GatewayTypingStart(
    Snowflake ChannelId,
    Snowflake UserId,
    [property: JsonConverter(typeof(JsonEpochTimeConverter))] 
    DateTimeOffset Timestamp,
    Snowflake? GuildId,
    GuildMember? Member
);