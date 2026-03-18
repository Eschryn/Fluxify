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
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.Emoji;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Roles;
using Fluxify.Dto.Guilds.Stickers;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;

namespace Fluxify.Gateway.Model.Data.Guild;

// https://github.com/fluxerapp/fluxer/blob/91ba0b096f6f39c717ebdadf48e39c394db21c8d/fluxer_gateway/src/guild/guild_data.erl#L114
public record GatewayGuildCreate(
    Snowflake Id,
    GuildResponse Properties,
    GuildRoleResponse[] Roles,
    ChannelResponse[] Channels,
    GuildEmojiResponse[] Emojis,
    GuildStickerResponse[] Stickers,
    GuildMemberResponse[] Members,
    int MemberCount,
    int OnlineCount,
    Presence[] Presences,
    VoiceStateResponse[] VoiceStates,
    DateTimeOffset JoinedAt
);