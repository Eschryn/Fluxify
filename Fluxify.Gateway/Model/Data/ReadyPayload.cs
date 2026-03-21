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

using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Voice;
using Fluxify.Dto.SavedMedia;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.GuildSettings;
using Fluxify.Dto.Users.Relationships;
using Fluxify.Dto.Users.Settings;
using Fluxify.Gateway.Model.Data.Channel;
using Fluxify.Gateway.Model.Data.Guild;
using Fluxify.Gateway.Model.Data.User;

namespace Fluxify.Gateway.Model.Data;

/// <summary>
/// Sent by the server after successful identification
/// </summary>
/// <param name="Version">The gateway protocol version</param>
/// <param name="SessionId">Identifier for the current session</param>
public record ReadyPayload(
    int Version,
    string SessionId,
    UserPrivateReponse User,
    GuildReadyData[] Guilds,
    ChannelResponse[] PrivateChannels,
    RelationshipResponse[] Relationships,
    UserPartialResponse[] Users,
    Presence[] Presences,
    GatewaySession[] Sessions,
    UserSettings? UserSettings,
    UserGuildSettingsResponse[]? UserGuildSettings,
    ReadState[]? ReadStates,
    Dictionary<string, string>? Notes,
    string? CountryCode,
    Snowflake[]? PinnedDms,
    FavoriteMemeResponse[]? FavoriteMemes,
    string? AuthSessionIdHash,
    RtcRegion[]? RtcRegions
);