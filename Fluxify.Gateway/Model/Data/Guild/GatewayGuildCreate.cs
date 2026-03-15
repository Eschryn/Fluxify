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
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Settings;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;

namespace Fluxify.Gateway.Model.Data.Guild;

public record GatewayGuildCreate(
    ChannelResponse[] Channels,
    GuildMemberResponse[] Members,
    Presence[] Presences,
    VoiceStateResponse[] VoiceStates,
    DateTimeOffset JoinedAt,
    Snowflake? AfkChannelId,
    int AfkTimeout, 
    string? BannerHash,
    int? BannerHeight,
    int? BannerWidth,
    DefaultMessageNotifications DefaultMessageNotifications,
    GuildOperations DisabledOperations,
    string? EmbedSplashHash,
    int? EmbedSplashHeight,
    int? EmbedSplashWidth,
    GuildExplicitContentFilter ExplicitContentFilter,
    GuildFeatureSchema[] Features,
    string? IconHash,
    Snowflake? Id,
    DateTimeOffset? MessageHistoryCutoff,
    GuildMfaLevel MfaLevel,
    string Name,
    NsfwLevel NsfwLevel,
    Snowflake OwnerId,
    Permissions? Permissions,
    Snowflake? RulesChannelId,
    string? SplashHash,
    SplashCardAlignment SplashCardAlignment,
    int? SplashHeight,
    int? SplashWidth,
    SystemChannelFlags SystemChannelFlags,
    Snowflake? SystemChannelId,
    string? VanityUrlCode,
    GuildVerificationLevel VerificationLevel
) : GuildResponse(
    AfkChannelId, 
    AfkTimeout, 
    BannerHash, 
    BannerHeight, 
    BannerWidth,
    DefaultMessageNotifications,
    DisabledOperations,
    EmbedSplashHash,
    EmbedSplashHeight,
    EmbedSplashWidth,
    ExplicitContentFilter,
    Features,
    IconHash,
    Id,
    MessageHistoryCutoff,
    MfaLevel,
    Name,
    NsfwLevel,
    OwnerId,
    Permissions,
    RulesChannelId,
    SplashHash,
    SplashCardAlignment,
    SplashHeight, 
    SplashWidth,
    SystemChannelFlags,
    SystemChannelId,
    VanityUrlCode,
    VerificationLevel
);