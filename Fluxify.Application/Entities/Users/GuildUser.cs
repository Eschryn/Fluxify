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
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.Repositories;
using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Members;

namespace Fluxify.Application.Entities.Users;

public class GuildUser : IUser, IEntity
{
    public Snowflake Id { get; init; }

    internal Snowflake[] AssignedRoleIds { get; set; } = [];
    public int? AccentColor { get; internal set;  }
    public string? AvatarHash { get; internal set;  }
    public string? BannerHash { get; internal set;  } 
    public DateTimeOffset? JoinedAt { get; internal set;  }
    public DateTimeOffset? CommunicationsDisabledUntil { get; internal set;  }
    public bool Deaf { get; internal set;  }
    public bool Mute { get; internal set;  }
    public string? Nick { get; internal set;  }
    public GuildMemberProfileFlags ProfileFlags { get; internal set;  }
    public IReadOnlyCollection<Role> Roles => AssignedRoleIds.Select(Guild.RolesRepository.Cache.GetCachedOrDefault<Role>).OfType<Role>().ToArray();
    public required Guild Guild { get; init; }
    internal IUser User { get; set; } = default!;
    public bool? Bot => User.Bot;
    public string Username => User.Username;
    public string? Discriminator => User.Discriminator;
    public string? GlobalName => User.GlobalName;
    public string? Avatar => User.Avatar;
    public Color? AvatarColor => User.AvatarColor;
    public bool? System => User.System;
    public PublicUserFlags Flags => User.Flags;
}