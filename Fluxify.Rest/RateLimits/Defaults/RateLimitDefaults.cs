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

/// <summary>
/// Contains default rate limit configuration.
/// </summary>
internal partial class RateLimitDefaults
{
    /// <summary>
    /// All rate limits set for the admin section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Admin { get; }
    
    /// <summary>
    /// All rate limits set for the auth section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Auth { get; }
    
    /// <summary>
    /// All rate limits set for the channel section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Channel { get; }
    
    /// <summary>
    /// All rate limits set for the connection section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Connection { get; }
    
    /// <summary>
    /// All rate limits set for the discovery section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Discovery { get; }
    
    /// <summary>
    /// All rate limits set for the donation section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Donation { get; }
    
    /// <summary>
    /// All rate limits set for the guild section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Guild { get; }
    
    /// <summary>
    /// All rate limits set for the integration section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Integration { get; }
    
    /// <summary>
    /// All rate limits set for the invite section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Invite { get; }
    
    /// <summary>
    /// All rate limits set for the misc section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Misc { get; }
    
    /// <summary>
    /// All rate limits set for the oauth section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> OAuth { get; }
    
    /// <summary>
    /// All rate limits set for the user section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> User { get; }
    
    /// <summary>
    /// All rate limits set for the webhook section.
    /// </summary>
    public static partial FrozenDictionary<string, TokenBucketRateLimiterOptions> Webhook { get; }
    
    /// <summary>
    /// All rate limits combined in one dictionary.
    /// </summary>
    public static FrozenDictionary<string, TokenBucketRateLimiterOptions> All { get; }

    /// <summary>
    /// Default global rate limit.
    /// </summary>
    public static TokenBucketRateLimiterOptions Global { get; } = new()
    {
        TokenLimit = 50,
        TokensPerPeriod = 50,
        ReplenishmentPeriod = TimeSpan.FromSeconds(1),
    };

    static RateLimitDefaults()
    {
        All = Admin
            .Concat(Auth)
            .Concat(Channel)
            .Concat(Connection)
            .Concat(Discovery)
            .Concat(Donation)
            .Concat(Guild)
            .Concat(Integration)
            .Concat(Invite)
            .Concat(Misc)
            .Concat(OAuth)
            .Concat(User)
            .Concat(Webhook)
            .Prepend(new KeyValuePair<string, TokenBucketRateLimiterOptions>("global", Global))
            .ToFrozenDictionary();
    }
}