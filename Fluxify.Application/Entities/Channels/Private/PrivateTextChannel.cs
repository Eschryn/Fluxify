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
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.Repositories;
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages.BulkDelete;
using Fluxify.Rest.Channel;

namespace Fluxify.Application.Entities.Channels.Private;

public abstract class PrivateTextChannel(
    FluxerApplication fluxerApplication
) : ITextChannel
{
    protected ChannelRequestBuilder RequestBuilder => field ??= FluxerApplication.Rest.Channels[Id];
    protected readonly FluxerApplication FluxerApplication = fluxerApplication;

    public required Snowflake Id { get; init; }
    public Snowflake? LastMessageId { get; internal set; }
    public DateTimeOffset? LastPinTimestamp { get; internal set; }
    internal CacheRef<GlobalUser>[] RecipientsRef { get; set; }
    public GlobalUser[] Recipients => RecipientsRef.Select(x => x.Value).OfType<GlobalUser>().ToArray();
    
    internal MessageRepository MessageRepository 
        => field ??= new MessageRepository(
            FluxerApplication,
            FluxerApplication.Rest.Channels[Id].Messages,
            Id,
            FluxerApplication.CacheConfig,
            LastMessageId
        );
    
    public async Task<Message?> SendMessageAsync(MessageCreate message, CancellationToken cancellationToken = default) 
        => await FluxerApplication.MessageMapper.MapAsync(
            await RequestBuilder.Messages.SendMessageAsync(FluxerApplication.MessageMapper.Map(message), cancellationToken)
                ?? throw new Exception("Message was not sent"));

    public async Task IndicateTypingAsync(CancellationToken cancellationToken = default)
        => await RequestBuilder.IndicateTypingAsync(cancellationToken);

    public IAsyncEnumerable<IReadOnlyList<Message>> GetMessagesAsync(
        Snowflake? start = null,
        Direction direction = Direction.Before,
        int limit = 100,
        int limitPerPage = 50,
        bool bypassCache = false,
        CancellationToken cancellationToken = default
    ) => MessageRepository.GetMessagesAsync(start, direction, limit, limitPerPage, bypassCache, cancellationToken);

    public Task<IReadOnlyList<Message>> GetMessagesAroundAsync(
        Snowflake around,
        int limit = 100,
        CancellationToken cancellationToken = default
    ) => MessageRepository.GetMessagesAroundAsync(around, limit, cancellationToken);

    public Task<Message> GetMessageAsync(
        Snowflake id,
        CancellationToken cancellationToken = default
    ) => MessageRepository.GetMessageAsync(id, cancellationToken);

    public Task DeleteMessageAsync(
        Snowflake id,
        CancellationToken cancellationToken = default
    ) => MessageRepository.DeleteMessageAsync(id, cancellationToken);
    
    public Task DeleteMessagesAsync(
        Snowflake[] ids,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.Messages.BulkDeleteAsync(new BulkDeleteMessagesRequest(ids), cancellationToken);
    
    public Task<Message> EditMessageAsync(
        Message message,
        Action<MessageEdit> edit,
        CancellationToken cancellationToken = default
    ) => MessageRepository.EditMessageAsync(message, edit, cancellationToken);
    
    public IAsyncEnumerable<IReadOnlyList<Message>> GetPinnedMessagesAsync(
        int limit = 100,
        int limitPerPage = 25,
        DateTimeOffset? before = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.Messages.GetPinnedMessagesAsync(
        FluxerApplication.MessageMapper,
        limit,
        limitPerPage,
        before,
        cancellationToken
    );
    
    public Task AckPinnedMessagesAsync(CancellationToken cancellationToken = default)
        => RequestBuilder.Messages.Pins.AcknowledgeAsync(cancellationToken);
    
    public Task MarkUnreadAsync(CancellationToken cancellationToken = default) 
        => RequestBuilder.Messages.MarkUnreadAsync(cancellationToken);

    public Task BulkDeleteMessagesAsync(Snowflake[] ids, CancellationToken cancellationToken = default)
        => RequestBuilder.Messages.BulkDeleteAsync(new BulkDeleteMessagesRequest(ids), cancellationToken);
    
    public Task ScheduleMessageAsync(
        MessageCreate message,
        DateTimeOffset scheduledTime,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.Messages.ScheduleMessageAsync(
        FluxerApplication.MessageMapper.Map(
            message,
            scheduledTime.LocalDateTime,
            TimeZoneInfo.Local.StandardName
        ),
        cancellationToken
    );
    
    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        "i" or "I" => ((long)Id).ToString(),
        _ or "m" or "M" => $"<#{Id}>"
    };

    public object Clone() => MemberwiseClone();
}