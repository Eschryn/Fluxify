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
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Guilds;
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
    public required string Content { get; set; }

    public Snowflake? WebhookId { get; internal set; }
    public required IUser Author { get; init; }
    public required TextChannel Channel { get; init; }
    public Attachment[]? Attachments { get; internal set; }
    public Embed[]? Embeds { get; set; }
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
    
    public GlobalUser[]? Mentions { get; internal set; }
    public Snowflake[]? MentionRoles { get; internal set; }
    
    public CallInfo? Call { get; internal set; }

    public MessageReference? MessageReference { get; internal set; }
    public MessageSnapshot[]? MessageSnapshots { get; internal set; }
    public Message? ReferencedMessage { get; internal set; }

    public async Task UpdateAsync()
    {
        throw new NotImplementedException();
    }
    
    public async Task ReplyAsync(MessageDto message, CancellationToken cancellationToken = default)
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

        await Channel.SendMessageAsync(message, cancellationToken);
    }

    public async Task ReactAsync(string unicodeEmoji, CancellationToken cancellationToken = default) 
        => await RequestBuilder.Reactions[unicodeEmoji].ReactAsync(application.Gateway.SessionId!, cancellationToken);
}