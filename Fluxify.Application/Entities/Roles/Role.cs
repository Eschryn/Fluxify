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

using System.Drawing;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Roles;

public class Role : IRole
{
    public Snowflake Id { get; internal set; }
    public required string Name { get; init; }
    public long Position { get; internal set; }
    public bool IsMentionable { get; internal set; }
    public bool Hoist { get; internal set; }
    public long? HoistPosition { get; internal set; }
    public string? UnicodeEmoji { get; internal set; }
    public Color Color { get; internal set; }
    public Permissions Permissions { get; internal set; }
    internal CacheRef<Guild> GuildRef { get; init; }
    public Guild Guild => GuildRef.Value;
    
    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        "i" or "I" => ((long)Id).ToString(),
        _ or "m" or "M" => $"<@&{Id}>"
    };
}