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
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Webhook
    {
        get => field;
    } = new Dictionary<string, TokenBucketRateLimiterOptions>()
    {
        ["webhook:list::guild_id"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["webhook:list::channel_id"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["webhook:create::channel_id"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["webhook:read::webhook_id"] = new()
        {
            TokenLimit = 100,
            TokensPerPeriod = 100,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["webhook:update::webhook_id"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["webhook:delete::webhook_id"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["webhook:execute::webhook_id"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["webhook:message_get::webhook_id"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["webhook:message_edit::webhook_id"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["webhook:message_delete::webhook_id"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["webhook:github::webhook_id"] = new()
        {
            TokenLimit = 200,
            TokensPerPeriod = 200,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["webhook:instatus::webhook_id"] = new()
        {
            TokenLimit = 200,
            TokensPerPeriod = 200,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
    }.ToFrozenDictionary();
}