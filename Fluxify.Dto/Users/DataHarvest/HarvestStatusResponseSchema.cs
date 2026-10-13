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
/// Response object for the request harvest status request.
/// </summary>
/// <param name="CompletedAt">When the harvest completed and became available for download.</param>
/// <param name="CreatedAt">When the harvest was requested.</param>
/// <param name="DownloadUrlExpiresAt">When the download link for the harvest expires.</param>
/// <param name="ErrorMessage">The failure message if the harvest failed, else null.</param>
/// <param name="ExpiresAt">When the harvest expires.</param>
/// <param name="FailedAt">When the harvest failed.</param>
/// <param name="FileSize">The size of the harvest archive.</param>
/// <param name="HarvestId">The id of the harvest.</param>
/// <param name="ProgressPercent">The progress of the harvest in percent.</param>
/// <param name="ProgressStep">Which step the harvest is in.</param>
/// <param name="StartedAt">When the harvest started processing.</param>
/// <param name="Status">The status of the harvest.</param>
/// <seealso href="https://docs.fluxer.app/http-api/users/data-harvest/#harvest-status-object"/>
public record HarvestStatusResponseSchema(
    Snowflake HarvestId,
    HarvestStatusEnum Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? StartedAt,
    DateTimeOffset? CompletedAt,
    DateTimeOffset? FailedAt,
    string? FileSize,
    double ProgressPercent,
    string? ProgressStep,
    string? ErrorMessage,
    DateTimeOffset? DownloadUrlExpiresAt,
    DateTimeOffset? ExpiresAt
);