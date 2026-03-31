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
using Fluxify.Core.Types;
using Fluxify.Dto.Users;

namespace Fluxify.Application.Entities.Users;

public class GlobalUser : IUser, IPresence
{
    public UserStatus Status { get; internal set; } = UserStatus.Offline;
    public bool IsMobile { get; internal set; } = false;
    public bool IsAfk { get; internal set; } = false;
    public CustomStatus? CustomStatus { get; internal set; } = null;
    public Snowflake Id { get; init; }
    public bool? Bot { get; internal set; }
    public string Username { get; internal set; } = string.Empty;
    public string? Discriminator { get; internal set; }
    public string? GlobalName { get; internal set; }
    public string? AvatarHash { get; internal set; }
    public Color? AvatarColor { get; internal set; }
    public bool? System { get; internal set; }
    public PublicUserFlags Flags { get; internal set; }
    
    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        "i" or "I" => ((long)Id).ToString(),
        _ or "m" or "M" => $"<#{Id}>"
    };
}