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

using System.Collections.Frozen;
using System.Threading.RateLimiting;

namespace Fluxify.Rest.RateLimits.Defaults;

partial class RateLimitDefaults
{
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Donation
    {
        get => field;
    } = new Dictionary<string, TokenBucketRateLimiterOptions>()
    {
        ["donation:request_link"] = new()
        {
            TokenLimit = 3,
            TokensPerPeriod = 3,
            ReplenishmentPeriod = TimeSpan.FromHours(1),
        },

        ["donation:request_link:email"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["donation:manage"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["donation:checkout"] = new()
        {
            TokenLimit = 5,
            TokensPerPeriod = 5,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["donation:checkout:email"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
    }.ToFrozenDictionary();
}