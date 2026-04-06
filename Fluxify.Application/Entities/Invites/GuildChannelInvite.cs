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

using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.State.Ref;

namespace Fluxify.Application.Entities.Invites;

public class GuildChannelInvite : IInvite
{
    public required CacheRef<IChannel> Channel { get; init; }
    public string Code { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    //GuildInviteMetadataResponseGuild Guild,
    public ICacheRef<IUser> Inviter { get; set; }
    public int MemberCount { get; set; }
    public int PresenceCount { get; set; }
    public bool Temporary { get; set; }
}