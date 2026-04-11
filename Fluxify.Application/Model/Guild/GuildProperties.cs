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

using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Application.Model.Guild;

public class GuildProperties
{
    public Snowflake? AfkChannelId { get; set; }
    public int? AfkTimeout { get; set; }
    public Base64Image? Banner { get; set; }
    public DefaultMessageNotifications? DefaultMessageNotifications { get; set; }
    public Base64Image? EmbedSplash { get; set; }
    public GuildExplicitContentFilter? ExplicitContentFilter { get; set; }
    public GuildFeatureSchema[]? Features { get; set; } = [];
    public Base64Image? Icon { get; set; }
    public DateTimeOffset? MessageHistoryCutoff { get; set; }
    public string? Name { get; set; }
    public NsfwLevel? NsfwLevel { get; set; }
    public GuildMfaLevel? MfaLevel { get; set; }
    public Base64Image? Splash { get; set; }
    public SplashCardAlignment? SplashCardAlignment { get; set; }
    public SystemChannelFlags? SystemChannelFlags { get; set; }
    public Snowflake? SystemChannelId { get; set; }
    public GuildVerificationLevel? VerificationLevel { get; set; }
}