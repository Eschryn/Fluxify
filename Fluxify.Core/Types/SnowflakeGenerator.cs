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

namespace Fluxify.Core.Types;

/// <summary>
/// Creates <see cref="Snowflake"/>s with <see name="processId"/> and <paramref name="workerId"/>.
/// </summary>
/// <param name="processId">The internal process ID.</param>
/// <param name="workerId">The worker ID.</param>
public sealed class SnowflakeGenerator(byte processId, byte workerId)
{
    /// <summary>
    /// The default <see cref="Snowflake"/> generator uses process ID 0 and worker ID 0.
    /// </summary>
    public static SnowflakeGenerator Default { get; } = new(0, 0);
    
    private uint _processCounter = 0;
    
    /// <summary>
    /// Creates a new <see cref="Snowflake"/>.
    /// </summary>
    /// <returns>The <see cref="Snowflake"/> that was created.</returns>
    public Snowflake Create()
    {
        var timestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - 1420070400000;
        var increment = (ulong)Interlocked.Increment(ref _processCounter);
        
        return new Snowflake((timestamp << 22) | ((ulong)workerId << 17 & 0x1F) | ((ulong)(processId & 0x1F) << 12) | increment & 0xFFF);
    }
}