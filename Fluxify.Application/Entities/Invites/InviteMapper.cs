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
using Fluxify.Dto.Guilds.Invite;
using Fluxify.Dto.Invites;

namespace Fluxify.Application.Entities.Invites;

[Mapper]
public partial class InviteMapper(FluxerApplication fluxerApplication)
{
    [UseMapper] private CacheMapper CacheMapper => fluxerApplication.CacheMapper;
    [UseMapper] private GuildMapper GuildMapper => fluxerApplication.GuildMapper;

    [MapDerivedType<GuildInviteResponse, GuildChannelInvite>]
    public partial IInvite MapFromResponse(InviteResponseSchema response);
    
    [MapDerivedType<GuildInviteMetadataResponse, GuildChannelInviteMetadata>]
    public partial IInviteMetadata MapFromResponse(InviteMetadataResponseSchema response);
    
    [MapPropertyFromSource(nameof(GuildInviteResponse.Inviter), Use = nameof(@CacheMapper.ResolveCachedInviter))]
    public partial GuildChannelInvite MapFromResponse(GuildInviteResponse response);
    
    [MapPropertyFromSource(nameof(GuildInviteResponse.Inviter), Use = nameof(@CacheMapper.ResolveCachedInviter))]
    public partial GuildChannelInviteMetadata MapFromResponse(GuildInviteMetadataResponse response);
}