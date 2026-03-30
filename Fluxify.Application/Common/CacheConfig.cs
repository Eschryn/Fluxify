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

namespace Fluxify.Application.Common;

public class CacheConfig
{
    /// <summary>
    /// The size of the message cache.
    /// </summary>
    /// <remarks>
    /// When cache limits are reached, the oldest message in the cache will be removed.<br/><br/>
    /// 0 => no cache
    /// </remarks>
    public long MessageCacheSize { get; set; } = 0;
    
    /// <summary>
    /// The size of the user cache.
    /// </summary>
    /// <remarks>
    /// When cache limits are reached, the least recently used user will be removed.<br/><br/>
    /// 0 => no cache<br/>
    /// long.MaxValue => permanent cache
    /// </remarks>
    public long UserCacheSize { get; set; } = long.MaxValue;
    
    /// <summary>
    /// The size of the guild user cache.
    /// </summary>
    /// <remarks>
    /// When cache limits are reached, the least recently used guild user will be removed.<br/><br/>
    /// 0 => no cache<br/>
    /// long.MaxValue => permanent cache
    /// </remarks>
    public long GuildUserCacheSize { get; set; } = long.MaxValue;
    
    // LRU Cache
    internal long GuildCacheSize { get; set; } = long.MaxValue;
    // LRU Cache
    internal long ChannelCacheSize { get; set; } = long.MaxValue;
}