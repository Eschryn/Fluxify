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

using System.Collections.Concurrent;
using Fluxify.Application.Common;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.State;
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Rest.Channel.Messages;

namespace Fluxify.Application.Repositories;

internal sealed class MessageRepository(
    FluxerApplication fluxerApplication,
    MessagesRequestBuilder messages,
    ITextChannel channel,
    CacheConfig config,
    Snowflake? lastMessageId = null
)
{
    private readonly MessagesRequestBuilder _messages = messages;
    private Snowflake _lastMessageId = lastMessageId ?? 0L;
private MessageViewOrchestrator _messageViewOrchestrator = new(fluxerApplication.MessageMapper);
internal MessageView TailView => field ??= _messageViewOrchestrator.CreateView(config.MessageCacheSize);
    
    internal ICache<Message> Cache = config.MessageCacheSize switch
    {
        > 0 => new OrderedCache<Message, MessageMapper>(fluxerApplication.MessageMapper, config.MessageCacheSize),
        _ => new PassthroughCache<Message>()
    };

    internal CacheRef<Message> InsertNew(MessageResponse response, ICacheRef<IUser>? author = null)
    {
        _lastMessageId = response.Id;
        return Insert(response, author);
    }

    private CacheRef<IChannel> SelfChannelRef { get; } = fluxerApplication.ChannelsRepository.Cache.GetCachedOrDefault(channel.Id);
    
    internal CacheRef<Message> Insert(MessageResponse response, ICacheRef<IUser>? author = null)
        => Cache.UpdateOrCreate(
            fluxerApplication.MessageMapper.Map(
                response,
                SelfChannelRef,
                author));

    public Message Update(MessageResponse messageResponse, ICacheRef<IUser>? user)
        => Cache.IsCached(messageResponse.Id) 
            ? Insert(messageResponse, user).Value!
            : fluxerApplication.MessageMapper.Map(messageResponse, SelfChannelRef, user);

    private async Task<IReadOnlyList<Message>> GetMessagesAsync(
        Snowflake? start,
        Direction direction,
        int limit,
        bool bypassCache,
        CancellationToken cancellationToken
    )
    {
        if (_lastMessageId == start && direction == Direction.After)
        {
            return [];
        }

        if (!bypassCache
            && Cache is OrderedCache<Message, MessageMapper> cache
            && cache.GetPaged(start, direction, limit) is { } page)
        {
            if (page.Count < limit)
            {
                var remainderStart = direction switch
                {
                    Direction.Before => page[^1].Id,
                    Direction.After => page[0].Id,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };

                // we're done
                if (remainderStart == _lastMessageId && direction == Direction.After)
                {
                    return page.Select(r => r.Value).OfType<Message>().ToArray().AsReadOnly();
                }

                var remainder =
                    await RequestMessagesAsync(remainderStart, direction, limit - page.Count, cancellationToken);

                var fullPage = direction switch
                {
                    Direction.Before => page.Select(r => r.Value).OfType<Message>().Concat(remainder),
                    Direction.After => remainder.Concat(page.Select(r => r.Value).OfType<Message>()),
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };

                return fullPage.ToArray().AsReadOnly();
            }

            return page.Select(r => r.Value).OfType<Message>().ToArray().AsReadOnly();
        }

        return await RequestMessagesAsync(start, direction, limit, cancellationToken);
    }

    private async Task<IReadOnlyList<Message>> RequestMessagesAsync(
        Snowflake? start,
        Direction direction,
        int limit,
        CancellationToken cancellationToken
    )
    {
        var (before, after) = direction switch
        {
            Direction.Before => (ValueTuple<Snowflake?, Snowflake?>)(start, null),
            Direction.After => (null, start),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        var response = await _messages
            .ListMessagesAsync(limit, before, after, null, cancellationToken);

        var messages = response?.Select(m => fluxerApplication.MessageMapper.Map(m)) ?? [];
        return messages.ToArray().AsReadOnly();
    }

    public async Task<Message> GetMessageAsync(
        Snowflake id,
        CancellationToken cancellationToken = default
    ) => await fluxerApplication.MessageMapper.MapAsync(
            await _messages[id].GetMessageAsync(cancellationToken) ?? throw new Exception("Message was not found"));

    public async Task DeleteMessageAsync(Snowflake id, CancellationToken cancellationToken = default)
    {
        Cache.Remove(id, out _);
        
        await _messages[id].DeleteMessageAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Message>> GetMessagesAroundAsync(
        Snowflake around,
        int limit = 100,
        CancellationToken cancellationToken = default
    )
    {
        var messages = await _messages
            .ListMessagesAsync(around: around, limit: limit, cancellationToken: cancellationToken);

        return (messages?.Select(m => fluxerApplication.MessageMapper.Map(m)).ToArray() ?? []).AsReadOnly();
    }

    public IAsyncEnumerable<IReadOnlyList<Message>> GetMessagesAsync(
        Snowflake? start = null,
        Direction direction = Direction.Before,
        int limit = 100,
        int limitPerPage = 50,
        bool bypassCache = false,
        CancellationToken cancellationToken = default
    )
    {
        var continuationIndex = direction == Direction.Before ? ^1 : 0;

        return new PagedRequest<Message, Snowflake>(
            limit,
            start,
            async id =>
            {
                var messages = await GetMessagesAsync(id, direction, limitPerPage, bypassCache, cancellationToken);
                return messages is { Count: > 0 }
                    ? new Page<Snowflake, Message>(messages[continuationIndex].Id, messages)
                    : null;
            }
        );
    }

    public async Task<Message> EditMessageAsync(
        Message message,
        Action<MessageEdit> edit,
        CancellationToken cancellationToken = default
    )
    {
        var messageEdit = fluxerApplication.MessageMapper.MapToEdit(message);
        edit(messageEdit);
        var request = fluxerApplication.MessageMapper.Map(messageEdit);
        var response = await _messages[message.Id].UpdateMessageAsync(request, cancellationToken);
        return response is not null ? Update(response, null) : throw new Exception("Message could not be updated.");
    }
}

public class MessageViewOrchestrator(MessageMapper mapper)
{
    private readonly ConcurrentDictionary<Snowflake, Message> _cache = new();
    
    public MessageView CreateView(long size) => new(size, mapper, this);

    internal IReadOnlyList<Message> ChangeView(
        Snowflake newHead,
        Snowflake newTail
    ) { throw new NotImplementedException();}

    internal MessageView[] ResolveMessageViews(
        Snowflake snowflake,
        Direction direction,
        int Count
    )
    {
        throw new NotImplementedException();
    }
}

public class MessageView(long size, MessageMapper mapper, MessageViewOrchestrator orchestrator)
{
    private ConcurrentQueue<Snowflake> _messageSequence = new();
    
    public Snowflake Head { get; }
    public Snowflake Tail { get; }
    
    public async Task LoadBeforeAsync(int count)
    {
        
    }
    
    public async Task LoadAfterAsync(int count)
    {
        
    }
}