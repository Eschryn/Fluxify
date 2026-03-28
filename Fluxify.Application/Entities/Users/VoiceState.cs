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

using Fluxify.Application.Entities.Channels.Guilds;

namespace Fluxify.Application.Entities.Users;

public class VoiceState : IVoiceState
{
    public required GuildVoiceChannel VoiceChannel { get; init; }
    public bool Mute { get; internal set; }
    public bool Deaf { get; internal set; }
    public bool? SelfStream { get; internal set; }
    public bool SelfDeaf { get; internal set; }
    public bool SelfMute { get; internal set; }
    public bool? SelfVideo { get; internal set; }
    public bool? IsMobile { get; internal set; }
    public string? SessionId { get; internal set; }
    public string ConnectionId { get; internal set; } = string.Empty;
    public int Version { get; internal set; }
    public string[]? ViewerStreamKeys { get; internal set; }
}