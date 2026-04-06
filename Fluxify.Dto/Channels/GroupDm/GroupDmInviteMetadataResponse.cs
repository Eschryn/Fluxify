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

using Fluxify.Dto.Invites;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Channels.GroupDm;

public record GroupDmInviteMetadataResponse(
    ChannelPartialResponse Channel,
    string Code,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt,
    UserPartialResponse? Inviter,
    int MaxUses,
    int MemberCount,
    bool Temporary,
    int Uses
) : InviteMetadataResponseSchema(
    Code,
    CreatedAt,
    ExpiresAt,
    Inviter,
    MaxUses,
    Temporary,
    Uses
);