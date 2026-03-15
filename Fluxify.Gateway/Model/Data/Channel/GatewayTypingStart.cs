using System.Text.Json.Serialization;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Gateway.Json;

namespace Fluxify.Gateway.Model.Data.Channel;

public record GatewayTypingStart(
    Snowflake ChannelId,
    Snowflake UserId,
    [property: JsonConverter(typeof(JsonEpochTimeConverter))] 
    DateTimeOffset Timestamp,
    Snowflake? GuildId,
    GuildMemberResponse? Member
);