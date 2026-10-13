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
using Fluxify.Dto.Json;

namespace Fluxify.Dto.Users.DataHarvest;

/// <summary>
/// States of a data harvest.
/// </summary>
/// <seealso href="https://docs.fluxer.app/http-api/users/data-harvest/#harvest-status-values"/>
[JsonConverter(typeof(JsonLowerCaseStringEnumConverter<HarvestStatusEnum>))]
public enum HarvestStatusEnum
{
    /// <summary>
    /// The harvest is awaiting processing.
    /// </summary>
    Pending,
    /// <summary>
    /// The harvest is currently being processed.
    /// </summary>
    Processing,
    /// <summary>
    /// The harvest was successfully completed and is available for download.
    /// </summary>
    Completed,
    /// <summary>
    /// The harvest failed to complete.
    /// </summary>
    Failed,
}