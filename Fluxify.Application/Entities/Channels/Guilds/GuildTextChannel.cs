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
using Fluxify.Application.Model.Channel;
using Fluxify.Application.Repositories;
using Fluxify.Application.State;

namespace Fluxify.Application.Entities.Channels.Guilds;

public partial class GuildTextChannel : GuildNestedChannel<GuildTextChannel, TextChannelProperties>, ITextChannel,
    ICloneable<GuildTextChannel>
{
    internal MessageRepository MessageRepository => field ??= new MessageRepository(
        FluxerApplication,
        FluxerApplication.Rest.Channels[Id].Messages,
        Id,
        FluxerApplication.CacheConfig,
        LastMessageId
    );

    public string? Topic { get; internal set; }
    public bool? Nsfw { get; internal set; }
    public int? RateLimitPerUser { get; internal set; }
    public Snowflake? LastMessageId { get; internal set; }
    public DateTimeOffset? LastPinTimestamp { get; internal set; }
    
    internal GuildTextChannel(
        FluxerApplication fluxerApplication,
        CacheRef<Guild> guildRef
    ) : base(fluxerApplication, guildRef)
    {
    }
}