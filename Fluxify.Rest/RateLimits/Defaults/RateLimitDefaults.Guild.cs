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
    public const string GuildEmojiCreate = "guild:emoji:create::guild_id";
    public const string GuildEmojiBulkCreate = "guild:emoji:bulk_create::guild_id";
    public const string GuildEmojiDelete = "guild:emoji:delete::guild_id";
    public const string GuildEmojiUpdate = "guild:emoji:update::guild_id";
    public const string GuildMembers = "guild:members::guild_id";
    public const string GuildEmojiClone = "guild:emoji:clone::guild_id";

    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Guild { get => field; } =
        new Dictionary<string, TokenBucketRateLimiterOptions>()
        {
            ["guild:create"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1)
            },
            ["guild:list"] = new()
            {
                TokenLimit = 40,
                TokensPerPeriod = 40,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:read::guild_id"] = new()
            {
                TokenLimit = 100,
                TokensPerPeriod = 100,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:update"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:update::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:delete::guild_id"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1)
            },
            ["guild:leave::guild_id"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:vanity_url:get::guild_id"] = new()
            {
                TokenLimit = 100,
                TokensPerPeriod = 100,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:vanity_url:patch::guild_id"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1)
            },
            [GuildMembers] = new()
            {
                TokenLimit = 40,
                TokensPerPeriod = 40,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:member:update::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:member:remove::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:member:role:add::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:member:role:remove::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:channels:list::guild_id"] = new()
            {
                TokenLimit = 60,
                TokensPerPeriod = 60,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:channel:create::guild_id"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1)
            },
            ["guild:channel:positions::guild_id"] = new()
            {
                TokenLimit = 30,
                TokensPerPeriod = 30,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:search::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:audit_logs::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:role:create::guild_id"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1)
            },
            ["guild:role:update::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:role:delete::guild_id"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1)
            },
            ["guild:role:positions::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:role:list::guild_id"] = new()
            {
                TokenLimit = 60,
                TokensPerPeriod = 60,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:role:hoist_positions::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:role:hoist_positions_reset::guild_id"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1)
            },
            ["guild:emojis:list::guild_id"] = new()
            {
                TokenLimit = 60,
                TokensPerPeriod = 60,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            [GuildEmojiCreate] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromSeconds(30)
            },
            [GuildEmojiBulkCreate] = new()
            {
                TokenLimit = 6,
                TokensPerPeriod = 6,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1)
            },
            [GuildEmojiClone] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromSeconds(30)
            },
            [GuildEmojiUpdate] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            [GuildEmojiDelete] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromSeconds(30)
            },
            ["guild:emoji:delete:daily::guild_id"] = new()
            {
                TokenLimit = 300,
                TokensPerPeriod = 300,
                ReplenishmentPeriod = TimeSpan.FromDays(1)
            },
            ["guild:emoji:metadata::user_id"] = new()
            {
                TokenLimit = 60,
                TokensPerPeriod = 60,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:emoji:source::user_id"] = new()
            {
                TokenLimit = 60,
                TokensPerPeriod = 60,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:sticker:list::guild_id"] = new()
            {
                TokenLimit = 60,
                TokensPerPeriod = 60,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:sticker:create::guild_id"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromSeconds(30)
            },
            ["guild:sticker:bulk_create::guild_id"] = new()
            {
                TokenLimit = 6,
                TokensPerPeriod = 6,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1)
            },
            ["guild:sticker:clone::guild_id"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromSeconds(30)
            },
            ["guild:sticker:update::guild_id"] = new()
            {
                TokenLimit = 20,
                TokensPerPeriod = 20,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:sticker:delete::guild_id"] = new()
            {
                TokenLimit = 10,
                TokensPerPeriod = 10,
                ReplenishmentPeriod = TimeSpan.FromSeconds(30)
            },
            ["guild:sticker:delete:daily::guild_id"] = new()
            {
                TokenLimit = 300,
                TokensPerPeriod = 300,
                ReplenishmentPeriod = TimeSpan.FromDays(1)
            },
            ["guild:sticker:metadata::user_id"] = new()
            {
                TokenLimit = 60,
                TokensPerPeriod = 60,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
            ["guild:sticker:source::user_id"] = new()
            {
                TokenLimit = 60,
                TokensPerPeriod = 60,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10)
            },
        }.ToFrozenDictionary();
}