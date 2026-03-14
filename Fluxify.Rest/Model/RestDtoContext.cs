using System.Text.Json.Serialization;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Pin;
using Fluxify.Dto.Channels.Voice;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.ScheduledMessages;

namespace Fluxify.Rest.Model;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(ErrorResponse))]
public partial class RestDtoContext : JsonSerializerContext;