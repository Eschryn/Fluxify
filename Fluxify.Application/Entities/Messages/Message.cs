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
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Core.Types;
using Fluxify.Rest.Channel;

namespace Fluxify.Application.Entities.Messages;

public class Message(
    FluxerApplication application
) : IEntity
{
    private MessageRequestBuilder RequestBuilder => field ??= application.Rest.Channels[Channel.Id].Messages[Id];

    public Snowflake Id { get; init; }
    public string? Content { get; internal set; }

    public Snowflake? WebhookId { get; internal set; }
    public required IUser Author { get; init; }
    public required ITextChannel Channel { get; init; }
    public Attachment[]? Attachments { get; internal set; }
    public Embed[]? Embeds { get; internal set; }
    public bool MentionsEveryone { get; internal set; }
    public Reaction[]? Reactions { get; internal set; }
    public Sticker[]? Stickers { get; internal set; }

    public DateTimeOffset CreatedAt { get; internal set; }
    public DateTimeOffset? EditedAt { get; internal set; }

    public string? Nonce { get; internal set; }

    public MessageFlags Flags { get; internal set; }
    public MessageType Type { get; internal set; }

    public bool IsPinned { get; internal set; }
    public bool? HasTts { get; internal set; }

    public IUser[]? Mentions { get; set; }
    internal Snowflake[] MentionRoles { get; set; } = Array.Empty<Snowflake>();
    
    public IRole[] MentionedRoles => Channel switch
    {
        IGuildChannel guildChannel
            => MentionRoles
                .Select(
                    id => guildChannel.Guild.RolesRepository.Cache.GetCachedOrDefault<IRole>(id))
                .OfType<IRole>()
                .ToArray(),
        _ => []
    };

    public CallInfo? Call { get; internal set; }

    public MessageReference? MessageReference { get; internal set; }
    public MessageSnapshot[]? MessageSnapshots { get; internal set; }
    public Message? ReferencedMessage { get; internal set; }

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

    private static string GetEmojiString(IEmoji emoji) => emoji switch
    {
        GuildEmoji ge => $"{ge.Name}:{ge.Id.ToString(CultureInfo.InvariantCulture)}",
        UnicodeEmoji ue => ue.Name,
        _ => throw new ArgumentOutOfRangeException(nameof(emoji), emoji, null)
    };

    public async Task ReactAsync(IEmoji emoji, CancellationToken cancellationToken = default)
        => await RequestBuilder.Reactions[GetEmojiString(emoji)]
            .ReactAsync(application.Gateway.SessionId!, cancellationToken);

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
            yield return userPartialResponses?.Select(application.Users.Insert).ToArray() ?? [];
        } while (lastId != null);
    }

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