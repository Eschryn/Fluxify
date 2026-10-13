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

namespace Fluxify.Dto.Users.DataHarvest;

/// <summary>
/// Filtering options for data harvests.
/// </summary>
/// <param name="Scope">The scope on which the harvest should operate.</param>
/// <param name="IncludeDms">Whether to include direct messages in the harvest.</param>
/// <param name="IncludeDmsClosed">Whether to include closed direct messages in the harvest.</param>
/// <param name="IncludeGroupDms">Whether to include group direct messages in the harvest.</param>
/// <param name="IncludeGuilds">Whether to include guild messages in the harvest.</param>
/// <param name="GuildFilterMode">The guild filter mode for the harvest.</param>
/// <param name="ExcludedGuildIds">Excluded guild ids from the harvest.</param>
/// <param name="IncludedGuildIds">Included guild ids for the harvest.</param>
/// <param name="StartDate">Messages that were sent after this date should be included in the harvest.</param>
/// <param name="EndDate">Messages that were sent before this date should be included in the harvest.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/data-harvest/#harvest-message-filter-object"/>
public record HarvestMessageFilter(
    HarvestMessageScope? Scope = null,
    bool? IncludeDms = null,
    bool? IncludeDmsClosed = null,
    bool? IncludeGroupDms = null,
    bool? IncludeGuilds = null,
    GuildFilterMode? GuildFilterMode = null,
    Snowflake[]? ExcludedGuildIds = null,
    Snowflake[]? IncludedGuildIds = null,
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null
);