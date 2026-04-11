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

using System.Collections.Concurrent;
using System.Drawing;
using Fluxify.Application.Common;
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.Entities.Users;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Rest.Guilds;

namespace Fluxify.Application.Entities.Guilds.Members;

public partial class GuildMember : IGuildMember
{
    private MemberRequestBuilder RequestBuilder => field ??= Guild.RequestBuilder.Members[Id];
    private readonly FluxerApplication _fluxerApplication;


    public Snowflake Id => UserRef.Id;
    internal Snowflake[] AssignedRoleIds { get; set; } = [];
    public Color? AccentColor { get; internal set;  }
    
    public Guild Guild => GuildRef.Value;
    [MapperIgnore]
    public GlobalUser User => UserRef.Value;

    public Image? Avatar
    {
        get => field ?? ImmutableUser!.Avatar;
        internal set;
    }

    public Image? Banner { get; internal set; }
    public DateTimeOffset? JoinedAt { get; internal set;  }
    public DateTimeOffset? CommunicationsDisabledUntil { get; internal set;  }
    public bool Deaf { get; internal set;  }
    public bool Mute { get; internal set;  }
    public string? Nick { get; internal set;  }
    public GuildMemberProfileFlags ProfileFlags { get; internal set;  }

    
    internal GlobalUser? ImmutableUser
    {
        get => field ??= UserRef.Value;
        set => field = null;
    }
    
    public bool? Bot => ImmutableUser!.Bot;
    public string Username => ImmutableUser!.Username;
    public string? Discriminator => ImmutableUser!.Discriminator;
    public string? GlobalName => ImmutableUser!.GlobalName;
    Image? IUser.Avatar => ImmutableUser!.Avatar;
    public Color? AvatarColor => ImmutableUser!.AvatarColor;
    public bool? System => ImmutableUser!.System;
    public PublicUserFlags Flags => ImmutableUser!.Flags;
    public IPresence? Presence  => ImmutableUser!.Presence;

    [MapperIgnore]
    internal readonly ConcurrentDictionary<string, VoiceState> VoiceStateList = [];

    [MapperIgnore]
    public IReadOnlyCollection<IVoiceState> VoiceStates => VoiceStateList.Values.Cast<IVoiceState>().ToArray();
    
    [MapperIgnore]
    public IReadOnlyCollection<IRole> Roles => AssignedRoleIds.Select(r => Guild.RolesRepository.Cache.GetCachedOrDefault(r).Value).OfType<IRole>().ToArray();
    
    internal CacheRef<Guild> GuildRef { get; }
    internal CacheRef<GlobalUser> UserRef { get; }

    internal GuildMember(
        FluxerApplication fluxerApplication,
        CacheRef<Guild> guildRef,
        CacheRef<GlobalUser> userRef)
    {
        _fluxerApplication = fluxerApplication;
        GuildRef = guildRef;
        UserRef = userRef;
    }
    
    public string ToString(string? format, IFormatProvider? formatProvider) => User.ToString(format, formatProvider);
    public object Clone() => MemberwiseClone();
}