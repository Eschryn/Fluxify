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
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Admin
    {
        get => field;
    } = new Dictionary<string, TokenBucketRateLimiterOptions>()
    {
        ["admin:lookup"] = new()
        {
            TokenLimit = 200,
            TokensPerPeriod = 200,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["admin:user:modify"] = new()
        {
            TokenLimit = 100,
            TokensPerPeriod = 100,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["admin:guild:modify"] = new()
        {
            TokenLimit = 100,
            TokensPerPeriod = 100,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["admin:ban:operation"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["admin:bulk:operation"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["admin:gateway:reload"] = new()
        {
            TokenLimit = 5,
            TokensPerPeriod = 5,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["admin:message:operation"] = new()
        {
            TokenLimit = 100,
            TokensPerPeriod = 100,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["admin:code:generation"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["admin:audit_log"] = new()
        {
            TokenLimit = 100,
            TokensPerPeriod = 100,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["admin:jobs:view"] = new()
        {
            TokenLimit = 600,
            TokensPerPeriod = 600,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["admin:general"] = new()
        {
            TokenLimit = 200,
            TokensPerPeriod = 200,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
    }.ToFrozenDictionary();
}