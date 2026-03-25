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

using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Channels.Text.Messages.Reactions;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Users;

namespace Fluxify.Gateway.Model.Data.Channel.Message;

public record GatewayMessage(
    Snowflake Id,
    Snowflake ChannelId,
    Snowflake? GuildId,
    GuildMemberResponse? Member,
    UserPartialResponse Author,
    Snowflake? WebhookId,
    MessageType Type,
    MessageFlags Flags,
    string Content,
    DateTimeOffset Timestamp,
    DateTimeOffset? EditedTimestamp,
    bool Pinned,
    bool MentionEveryone,
    bool? Tts,
    UserPartialResponse[]? Mentions,
    Snowflake[]? MentionRoles,
    MessageEmbedResponse[]? Embeds,
    MessageAttachmentResponse[]? Attachments,
    MessageStickerResponse[]? Stickers,
    MessageReactionResponse[]? Reactions,
    MessageReferenceResponse? MessageReference,
    MessageSnapshotResponse[]? MessageSnapshots,
    string? Nonce,
    MessageCallResponse? Call,
    MessageBaseResponse? ReferencedMessage,
    ChannelType ChannelType
) : MessageResponse(
    Id,
    ChannelId,
    Author,
    WebhookId,
    Type,
    Flags,
    Content,
    Timestamp,
    EditedTimestamp,
    Pinned,
    MentionEveryone,
    Tts,
    Mentions,
    MentionRoles,
    Embeds,
    Attachments,
    Stickers,
    Reactions,
    MessageReference,
    MessageSnapshots,
    Nonce,
    Call,
    ReferencedMessage
);