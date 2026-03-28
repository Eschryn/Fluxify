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

using Fluxify.Application.Common;
using Fluxify.Application.Entities.Messages;
using Fluxify.Rest.Channel.Messages;

namespace Fluxify.Application.Entities.Channels;

internal static class RequestBuilderExtensions
{
    extension(MessagesRequestBuilder messagesRequestBuilder)
    {
        internal IAsyncEnumerable<IReadOnlyList<Message>> GetPinnedMessagesAsync(
            MessageMapper messageMapper,
            int limit = 100,
            int limitPerPage = 25,
            DateTimeOffset? before = null,
            CancellationToken cancellationToken = default
        ) => new PagedRequest<Message, DateTimeOffset>(
            limit,
            before,
            async start =>
            {
                var pins = await messagesRequestBuilder.Pins
                    .ListPinsAsync(limitPerPage, start, cancellationToken);
            
                return pins is { HasMore: true } 
                    ? new Page<DateTimeOffset, Message>(
                        pins.Items[0].PinnedAt,
                        (
                            await Task.WhenAll(
                                pins.Items.Select(m => messageMapper.MapAsync(m.Message))
                            )
                        ).AsReadOnly()   
                    )   
                    : null;
            }
        );
    }
}