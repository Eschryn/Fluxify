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
    public const string UserEmailChangeStart = "user:email_change:start";
    public const string UserEmailChangeResendOriginal = "user:email_change:resend_original";
    public const string UserEmailChangeVerifyOriginal = "user:email_change:verify_original";
    public const string UserEmailChangeRequestNew = "user:email_change:request_new";
    public const string UserEmailChangeResendNew = "user:email_change:resend_new";
    public const string UserEmailChangeVerifyNew = "user:email_change:verify_new";
    public const string UserEmailChangeApply = "user:email_change:apply";
    public const string UserEmailChangeBouncedRequestNew = "user:email_change:bounced:request_new";
    public const string UserEmailChangeBouncedResendNew = "user:email_change:bounced:resend_new";
    public const string UserEmailChangeBouncedVerifyNew = "user:email_change:bounced:verify_new";

    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> User
    {
        get => field;
    } = new Dictionary<string, TokenBucketRateLimiterOptions>()
    {
        ["user:read::user_id"] = new()
        {
            TokenLimit = 100,
            TokensPerPeriod = 100,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:profile::target_id"] = new()
        {
            TokenLimit = 100,
            TokensPerPeriod = 100,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:check_tag"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:update"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        [UserEmailChangeStart] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        [UserEmailChangeResendOriginal] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        [UserEmailChangeVerifyOriginal] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        [UserEmailChangeRequestNew] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        [UserEmailChangeResendNew] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        [UserEmailChangeVerifyNew] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        [UserEmailChangeApply] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        [UserEmailChangeBouncedRequestNew] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        [UserEmailChangeBouncedResendNew] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        [UserEmailChangeBouncedVerifyNew] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:password_change:start"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:password_change:resend"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:password_change:verify"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:password_change:complete"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:account:disable"] = new()
        {
            TokenLimit = 5,
            TokensPerPeriod = 5,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["user:account:delete"] = new()
        {
            TokenLimit = 5,
            TokensPerPeriod = 5,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["user:phone_gate_escape:preview"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:phone_gate_escape:execute"] = new()
        {
            TokenLimit = 5,
            TokensPerPeriod = 5,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["user:data:harvest"] = new()
        {
            TokenLimit = 5,
            TokensPerPeriod = 5,
            ReplenishmentPeriod = TimeSpan.FromMinutes(30)
        },
        ["user:preload_messages"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:messages:bulk_delete"] = new()
        {
            TokenLimit = 6,
            TokensPerPeriod = 6,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:messages:bulk_delete_mine_filtered"] = new()
        {
            TokenLimit = 5,
            TokensPerPeriod = 5,
            ReplenishmentPeriod = TimeSpan.FromMinutes(30)
        },
        ["user:settings:get"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:settings:update"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:guild_settings:update"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:channels"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:group_dm:create"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["user:group_dm:recipient:add"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromHours(1)
        },
        ["user:relationships:list"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:friend_request:send"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:friend_request:accept"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:block"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:friend_request:bulk_ignore"] = new()
        {
            TokenLimit = 5,
            TokensPerPeriod = 5,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:relationship:delete"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:relationship:update"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:notes:read"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:notes:write"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:mentions:read"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:mentions:delete"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:saved_messages:read"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:saved_messages:write"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:mfa:totp:enable"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:mfa:totp:disable"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:authorized_ips:forget"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:mfa:backup_codes"] = new()
        {
            TokenLimit = 6,
            TokensPerPeriod = 6,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:mfa:backup_codes_challenge:start"] = new()
        {
            TokenLimit = 6,
            TokensPerPeriod = 6,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:mfa:backup_codes_challenge:resend"] = new()
        {
            TokenLimit = 6,
            TokensPerPeriod = 6,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:mfa:backup_codes_challenge:verify"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:mfa:backup_codes_challenge:regenerate"] = new()
        {
            TokenLimit = 6,
            TokensPerPeriod = 6,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:push:subscribe"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:push:unsubscribe"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:push:list"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:harvest:latest"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:harvest:status"] = new()
        {
            TokenLimit = 40,
            TokensPerPeriod = 40,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10)
        },
        ["user:harvest:download"] = new()
        {
            TokenLimit = 10,
            TokensPerPeriod = 10,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:harvest:download_file"] = new()
        {
            TokenLimit = 60,
            TokensPerPeriod = 60,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:entrance_sound:list"] = new()
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
        ["user:entrance_sound:upload"] = new()
        {
            TokenLimit = 5,
            TokensPerPeriod = 5,
            ReplenishmentPeriod = TimeSpan.FromMinutes(5)
        },
        ["user:entrance_sound:mutate"] = new()
        {
            TokenLimit = 20,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1)
        },
    }.ToFrozenDictionary();
}