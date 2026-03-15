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

using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Messages;

public class Message : IEntity
{
    public Snowflake Id { get; init; }
    public Snowflake ChannelId { get; init; }
    public string Content { get; set; }

    public Snowflake AuthorId { get; init; }
    public Snowflake? WebhookId { get; init; }
    public required PartialUser Author { get; init; }
    public required TextChannel Channel { get; init; }
    public Attachment[]? Attachments { get; init; }
    public Embed[]? Embeds { get; set; }
    public bool MentionsEveryone { get; init; }
    public Reaction[]? Reactions { get; init; }
    public Sticker[]? Stickers { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? EditedAt { get; init; }

    public string? Nonce { get; init; }

    public MessageFlags Flags { get; init; }
    public MessageType Type { get; init; }

    public bool IsPinned { get; init; }
    public bool? HasTts { get; init; }

    // MessageCallResponse? Call, this is only relevant for group dms

    //this is all references
    MessageReference MessageReference { get; init; }
    //MessageSnapshotResponse[]? MessageSnapshots, // forwarded
    // MessageBaseResponseSchema? ReferredMessage, // replied to

    public async Task UpdateAsync()
    {
    }
    
    
    public async Task ReplyAsync(MessageDto message)
    {
        message.MessageReference = new MessageReference
        {
            MessageId = Id,
            ChannelId = ChannelId,
            GuildId = Channel is IGuildChannel guildChannel 
                ? guildChannel.GuildId 
                : null,
            Type = MessageReferenceType.Reply
        };

        await Channel.SendMessageAsync(message);
    }

}