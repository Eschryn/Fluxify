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
using Fluxify.Application.Entities.Invites;
using Fluxify.Application.Model.Channel;
using Fluxify.Dto.Channels;

namespace Fluxify.Application.Entities.Channels.Guilds;

public class GuildNestedChannel<TSelf, TProperties> : GuildChannel<TSelf, TProperties>, INestedChannel
    where TProperties : ChannelProperties
    where TSelf : GuildNestedChannel<TSelf, TProperties>
{
    internal CacheRef<IChannel>? ParentRef;

    internal GuildNestedChannel(
        FluxerApplication fluxerApplication,
        CacheRef<Guild> guildRef
    ) : base(fluxerApplication, guildRef)
    {
    }

    public GuildCategory? Parent => ParentRef?.Value as GuildCategory;

    public async Task<GuildChannelInviteMetadata> CreateInviteAsync(
        TimeSpan? maxAge = null,
        int maxUses = 0,
        bool temporary = false,
        bool unique = false
    ) => (GuildChannelInviteMetadata)FluxerApplication.InviteMapper.MapFromResponse(
        await RequestBuilder.CreateInviteAsync(
            new ChannelInviteCreateRequest(
                maxAge?.Seconds,
                maxUses,
                temporary,
                unique
            )
        )
    );
}