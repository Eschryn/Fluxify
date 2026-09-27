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
/// Various helper properties for <see cref="Snowflake"/>
/// </summary>
public static class SnowflakeExtensions
{
    extension(Snowflake snowflake)
    {
        /// <summary>
        /// The timestamp of the <see cref="Snowflake"/> relative to the Fluxer epoch.
        /// </summary>
        public ulong FluxerEpochMs => (ulong)snowflake >> 22;
        
        /// <summary>
        /// The timestamp of the <see cref="Snowflake"/> in the Unix epoch.
        /// </summary>
        public long UnixEpochMs => (long)(snowflake.FluxerEpochMs + 1420070400000);
        
        /// <summary>
        /// The worker ID that generated the <see cref="Snowflake"/>.
        /// </summary>
        public byte WorkerId => (byte)(((ulong)snowflake & 0x3E0000) >> 17);
        
        /// <summary>
        /// The internal process ID that generated the <see cref="Snowflake"/>.
        /// </summary>
        public byte InternalProcessId => (byte)(((ulong)snowflake & 0x1F000) >> 12);
        
        /// <summary>
        /// The process internal count/id of the <see cref="Snowflake"/>.
        /// </summary>
        public ushort ScopedId => (ushort)((ulong)snowflake & 0xFFF);
    }
}