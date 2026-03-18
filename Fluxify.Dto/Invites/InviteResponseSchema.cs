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

using System.Text.Json.Serialization;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Common;
using Fluxify.Dto.Guilds.Invite;
using Fluxify.Dto.Packs;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Invites;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(GuildInviteResponse), (int)InviteType.Guild)]
[JsonDerivedType(typeof(GroupDmInviteResponse), (int)InviteType.GroupDm)]
[JsonDerivedType(typeof(PackInviteResponse), (int)InviteType.EmojiPackInvite)]
[JsonDerivedType(typeof(PackInviteResponse), (int)InviteType.StickerPackInvite)]
public record InviteResponseSchema(
    string Code,
    UserPartialResponse? Inviter,
    bool Temporary
);