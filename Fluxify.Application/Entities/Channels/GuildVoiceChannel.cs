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

using Fluxify.Application.Entities.Guilds;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Channels;

public class GuildVoiceChannel(FluxerApplication fluxerApplication) : IGuildChannel
{
    public Snowflake Id { get; init; }
    public required string Name { get; set; }
    public int Bitrate { get; init; }
    public int? UserLimit { get; init; }
    public Snowflake GuildId { get; init; }
    public Snowflake? RtcRegion { get; init; }
    public Guild? Guild { get; }
    public GuildCategory? Parent { get; init; }
    public int? Position { get; init; }
    public PermissionOverwrite[]? Overwrites { get; init; }
}