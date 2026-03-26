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

using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Model.Messages;
using Fluxify.Core.Types;
using Fluxify.Rest.Channel;

namespace Fluxify.Application.Entities.Channels;

public interface ITextChannel : IChannel
{
    public Snowflake? LastMessageId { get; }
    public DateTimeOffset? LastPinTimestamp { get; }

    Task<Message?> SendMessageAsync(MessageCreate message, CancellationToken cancellationToken = default);
    Task IndicateTypingAsync(CancellationToken cancellationToken = default);

    IAsyncEnumerable<IReadOnlyList<Message>> GetMessagesAsync(
        Snowflake? start = null,
        Direction direction = Direction.Before,
        int limit = 100,
        int limitPerPage = 50,
        bool bypassCache = false,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<Message>> GetMessagesAroundAsync(
        Snowflake around,
        int limit = 100,
        CancellationToken cancellationToken = default
    );

    Task<Message> GetMessageAsync(Snowflake id, CancellationToken cancellationToken = default);
    Task DeleteMessageAsync(Snowflake id, CancellationToken cancellationToken = default);

    IAsyncEnumerable<IReadOnlyList<Message>> GetPinnedMessagesAsync(int limit = 100,
        int limitPerPage = 25,
        DateTimeOffset? before = null,
        CancellationToken cancellationToken = default
    );

    Task<Message> EditMessageAsync(
        Message message,
        Action<MessageEdit> edit,
        CancellationToken cancellationToken = default
    );
    
    Task MarkUnreadAsync(CancellationToken cancellationToken = default);
    Task BulkDeleteMessagesAsync(Snowflake[] ids, CancellationToken cancellationToken = default);

    public Task ScheduleMessageAsync(
        MessageCreate message,
        DateTimeOffset scheduledTime,
        CancellationToken cancellationToken = default
    );

    Task AckPinnedMessagesAsync(CancellationToken cancellationToken = default);
}