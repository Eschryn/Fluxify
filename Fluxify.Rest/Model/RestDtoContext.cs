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
[JsonSerializable(typeof(UpdateCallRegionRequest))]
[JsonSerializable(typeof(RingRequest))]
[JsonSerializable(typeof(UserPartialResponse[]))]
[JsonSerializable(typeof(MessageResponse[]))]
[JsonSerializable(typeof(ScheduleMessageResponseSchema))]
[JsonSerializable(typeof(ScheduledMessageResponseSchemaPayload))]
[JsonSerializable(typeof(ChannelPinsResponse))]
[JsonSerializable(typeof(CallEligibilityResponse))]
[JsonSerializable(typeof(UpdateCallRegionRequest))]
[JsonSerializable(typeof(RingRequest))]
[JsonSerializable(typeof(ChannelPermissionOverwrite))]
[JsonSerializable(typeof(RtcRegion[]))]
public partial class RestDtoContext : JsonSerializerContext;