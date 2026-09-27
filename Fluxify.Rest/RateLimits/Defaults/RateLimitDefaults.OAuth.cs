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
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> OAuth
    {
        get => field;
    } = new Dictionary<string, TokenBucketRateLimiterOptions>()
    {
        ["oauth:authorize"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth:consent"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth:token"] = new()
        {
            TokenLimit = 120,
            TokensPerPeriod = 120,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth:introspect"] = new()
        {
            TokenLimit = 120,
            TokensPerPeriod = 120,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth:revoke"] = new()
        {
            TokenLimit = 120,
            TokensPerPeriod = 120,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth_dev:clients:list"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth_dev:clients:create"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["oauth_dev:clients:update::client_id"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth_dev:clients:rotate_secret::client_id"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["oauth_dev:clients:delete::client_id"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["oauth_dev:teams:list"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth_dev:teams:create"] = new()
        {
            TokenLimit = 15,
            TokensPerPeriod = 15,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["oauth_dev:teams:delete::team_id"] = new()
        {
            TokenLimit = 15,
            TokensPerPeriod = 15,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["oauth_dev:teams:members:list::team_id"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth_dev:teams:members:add::team_id"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth_dev:teams:members:remove::team_id::user_id"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth_dev:bot_tokens:list::client_id"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth_dev:bot_tokens:create::client_id"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["oauth_dev:bot_tokens:revoke"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
    }.ToFrozenDictionary();
}