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

using System.Globalization;
using System.Runtime.CompilerServices;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Messages;

namespace Fluxify.Application.Entities.Messages;

public partial class Message
{
    public Task EditAsync(
        Action<MessageEdit> edit,
        CancellationToken cancellationToken = default
    ) => Channel.EditMessageAsync(this, edit, cancellationToken);

    public async Task<Message?> ReplyAsync(MessageCreate message, CancellationToken cancellationToken = default)
    {
        message.MessageReference = new MessageReference
        {
            MessageId = Id,
            ChannelId = Channel.Id,
            GuildId = Channel is IGuildChannel guildChannel
                ? guildChannel.Guild.Id
                : null,
            Type = MessageReferenceType.Reply
        };

        return await Channel.SendMessageAsync(message, cancellationToken);
    }

    public async Task<Message?> ReplyAsync(Action<MessageBuilder> builder,
        CancellationToken cancellationToken = default)
    {
        var messageBuilder = new MessageBuilder();
        builder(messageBuilder);
        return await ReplyAsync(messageBuilder.Build(), cancellationToken);
    }

    private static string GetEmojiString(IEmoji emoji) => emoji switch
    {
        GuildEmoji ge => $"{ge.Name}:{ge.Id.ToString(CultureInfo.InvariantCulture)}",
        UnicodeEmoji ue => ue.Name,
        _ => throw new ArgumentOutOfRangeException(nameof(emoji), emoji, null)
    };

    public async Task ReactAsync(IEmoji emoji, CancellationToken cancellationToken = default)
        => await RequestBuilder.Reactions[GetEmojiString(emoji)]
            .ReactAsync(application.Gateway.SessionId!, cancellationToken);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task ReactAsync(UnicodeEmoji emoji, CancellationToken cancellationToken = default)
        => ReactAsync((IEmoji)emoji, cancellationToken);

    public async Task RemoveReactionAsync(IEmoji emoji, IUser user,
        CancellationToken cancellationToken = default)
    {
        var messageReactionRequestBuilder = RequestBuilder.Reactions[GetEmojiString(emoji)];

        if (user.Id == application.CurrentUser.Id)
        {
            await messageReactionRequestBuilder.RemoveOwnReactionAsync(application.Gateway.SessionId!,
                cancellationToken);
        }
        else
        {
            await messageReactionRequestBuilder.RemoveReactionAsync(user.Id.ToString(CultureInfo.InvariantCulture),
                cancellationToken);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task RemoveReactionAsync(UnicodeEmoji emoji, IUser user, CancellationToken cancellationToken = default)
        => RemoveReactionAsync((IEmoji)emoji, user, cancellationToken);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IAsyncEnumerable<ICollection<IUser>> GetReactionUsersAsync(
        UnicodeEmoji emoji,
        int limit = 100,
        CancellationToken cancellationToken = default
    ) => GetReactionUsersAsync((IEmoji)emoji, limit, cancellationToken);

    public async IAsyncEnumerable<ICollection<IUser>> GetReactionUsersAsync(
        IEmoji emoji,
        int limit = 100,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        Snowflake? lastId = null;
        do
        {
            var userPartialResponses = await RequestBuilder.Reactions[GetEmojiString(emoji)]
                .ListUsersAsync(
                    limit,
                    lastId,
                    cancellationToken
                );

            lastId = userPartialResponses?.LastOrDefault()?.Id;
            yield return userPartialResponses?.Select(r => application.UsersRepository.Insert(r).Value!).ToArray() ??
                         [];
        } while (lastId != null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task RemoveAllReactionsAsync(UnicodeEmoji emoji, CancellationToken cancellationToken = default)
        => RemoveAllReactionsAsync((IEmoji)emoji, cancellationToken);

    public async Task RemoveAllReactionsAsync(IEmoji emoji, CancellationToken cancellationToken = default)
        => await RequestBuilder.Reactions[GetEmojiString(emoji)].RemoveAllAsync(cancellationToken);

    public async Task RemoveAllReactionsAsync(CancellationToken cancellationToken = default)
        => await RequestBuilder.Reactions.RemoveAllReactionsAsync(cancellationToken);

    public async Task PinAsync(CancellationToken cancellationToken = default)
        => await application.Rest.Channels[Channel.Id].Messages.Pins[Id].PinAsync(cancellationToken);

    public async Task UnpinAsync(CancellationToken cancellationToken = default)
        => await application.Rest.Channels[Channel.Id].Messages.Pins[Id].UnpinAsync(cancellationToken);

    public async Task AckAsync(CancellationToken cancellationToken = default)
        => await RequestBuilder.AckMessageAsync(cancellationToken);

    public Task DeleteAttachmentAsync(Snowflake attachmentId, CancellationToken cancellationToken = default)
        => RequestBuilder.DeleteAttachmentAsync(attachmentId, cancellationToken);
}