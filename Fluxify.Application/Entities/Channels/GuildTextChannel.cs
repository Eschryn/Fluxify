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
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Channels;

public class GuildTextChannel(FluxerApplication fluxerApplication) : GuildNestedChannel(fluxerApplication), ITextChannel, INestedChannel
{
    public string? Topic { get; internal set; }
    public bool? Nsfw { get; internal set; }
    public int? RateLimitPerUser { get; internal set; }
    public Snowflake? LastMessageId { get; internal set; }
    public DateTimeOffset? LastPinTimestamp { get; internal set; }
    
    public async Task<Message?> SendMessageAsync(MessageDto message, CancellationToken cancellationToken = default)
        => await FluxerApplication.MessageMapper.MapAsync(
            await RequestBuilder.Messages.SendMessageAsync(FluxerApplication.MessageMapper.Map(message),
                cancellationToken)
            ?? throw new Exception("Message was not sent"));

    public Task IndicateTypingAsync(CancellationToken cancellationToken = default) => RequestBuilder.IndicateTypingAsync(cancellationToken);

    public async Task<Message> GetMessageAsync(Snowflake id, CancellationToken cancellationToken = default)
        => await FluxerApplication.MessageMapper.MapAsync(
            await RequestBuilder.Messages[id].GetMessageAsync(cancellationToken) ??
            throw new Exception("Message was not found"));
}