// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Text.Json.Serialization;
using Fluxify.Gateway.Model.Data;
using Fluxify.Gateway.Model.Data.Channel;
using Fluxify.Gateway.Model.Data.Channel.Message;
using Fluxify.Gateway.Model.Data.Channel.Reaction;
using Fluxify.Gateway.Model.Data.Guild;
using Fluxify.Gateway.Model.Data.Guild.Roles;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;

namespace Fluxify.Gateway.Model;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    NumberHandling = JsonNumberHandling.AllowReadingFromString,
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower
)]
[JsonSerializable(typeof(GatewayPayload))]
[JsonSerializable(typeof(IdentifyPayloadData))]
[JsonSerializable(typeof(HelloPayloadData))]
[JsonSerializable(typeof(ReadyPayload))]
[JsonSerializable(typeof(ResumePayloadData))]
[JsonSerializable(typeof(GatewayUserNoteUpdate))]
[JsonSerializable(typeof(GatewayMemeIdResponse))]
[JsonSerializable(typeof(GatewayMessageIdResponse))]
[JsonSerializable(typeof(GatewayAuthSessionChange))]
[JsonSerializable(typeof(Presence))]
[JsonSerializable(typeof(GatewayGuildCreate))]
[JsonSerializable(typeof(GatewayGuildDelete))]
[JsonSerializable(typeof(GatewayGuildMemberDelete))]
[JsonSerializable(typeof(GatewayGuildRole))]
[JsonSerializable(typeof(GatewayGuildRoleDelete))]
[JsonSerializable(typeof(GatewayGuildRoleBulk))]
[JsonSerializable(typeof(GatewayGuildMember))]
[JsonSerializable(typeof(GatewayEmojiUpdate))]
[JsonSerializable(typeof(GatewayStickerUpdate))]
[JsonSerializable(typeof(GatewayBanData))]
[JsonSerializable(typeof(GatewayBulkChannelUpdate))]
[JsonSerializable(typeof(GatewayChannelPinsUpdate))]
[JsonSerializable(typeof(GatewayChannelPinsAck))]
[JsonSerializable(typeof(GatewayMessageCreate))]
[JsonSerializable(typeof(GatewayMessageDelete))]
[JsonSerializable(typeof(GatewayMessageDeleteBulk))]
[JsonSerializable(typeof(GatewayReaction))]
[JsonSerializable(typeof(GatewayReactionRemoveAll))]
[JsonSerializable(typeof(GatewayReactionRemoveEmoji))]
[JsonSerializable(typeof(GatewayMessageAck))]
[JsonSerializable(typeof(GatewayTypingStart))]
[JsonSerializable(typeof(GatewayChannelId))]
[JsonSerializable(typeof(GatewayInviteDelete))]
[JsonSerializable(typeof(GatewayRelationshipId))]
[JsonSerializable(typeof(GatewayVoiceServer))]
[JsonSerializable(typeof(VoiceStateResponse))]
[JsonSerializable(typeof(GatewayCallSchema))]
[JsonSerializable(typeof(GatewaySession[]))]
[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(UpdateVoiceState))]
public partial class GatewayJsonContext : JsonSerializerContext;