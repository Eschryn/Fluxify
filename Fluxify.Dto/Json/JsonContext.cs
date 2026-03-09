using System.Text.Json.Serialization;
using Fluxify.Core;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.LinkChannel;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.SavedMedia;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.GuildSettings;
using Fluxify.Dto.Users.Relationships;
using Fluxify.Dto.Users.Settings;

namespace Fluxify.Dto.Json;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(Snowflake))]
[JsonSerializable(typeof(Snowflake[]))]
[JsonSerializable(typeof(UserPrivate))]
[JsonSerializable(typeof(UserSessionResponse))]
[JsonSerializable(typeof(UserGuildSettings))]
[JsonSerializable(typeof(FavoriteMemeResponse))]
[JsonSerializable(typeof(GuildResponse))]
[JsonSerializable(typeof(GuildMember))]
[JsonSerializable(typeof(ChannelResponse))]
[JsonSerializable(typeof(Relationship))]
[JsonSerializable(typeof(UserSettings))]
[JsonSerializable(typeof(Message))]
[JsonSerializable(typeof(ChannelCreateRequest))]
[JsonSerializable(typeof(ChannelUpdateRequest))]
public partial class DtoJsonContext : JsonSerializerContext;