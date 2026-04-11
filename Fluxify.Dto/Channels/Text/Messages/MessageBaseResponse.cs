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

using System.Text.Json.Serialization;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds.Response;
using Fluxify.Dto.Channels.Text.Messages.Reactions;
using Fluxify.Dto.Channels.Text.Messages.Reference;
using Fluxify.Dto.Common;
using Fluxify.Dto.Users;

namespace Fluxify.Dto.Channels.Text.Messages;

public record MessageBaseResponse(
    Snowflake Id,
    Snowflake ChannelId,
    UserPartialResponse Author,
    Snowflake? WebhookId,
    MessageType Type,
    MessageFlags Flags,
    string Content,
    DateTimeOffset CreatedAt,
    DateTimeOffset? EditedAt,
    bool IsPinned,
    bool HasEveryoneMention,
    bool? HasTts,
    UserPartialResponse[]? Mentions,
    Snowflake[]? MentionRoles,
    MessageEmbedResponse[]? Embeds,
    MessageAttachmentResponse[]? Attachments,
    StickerResponse[]? Stickers,
    MessageReactionResponse[]? Reactions,
    MessageReferenceResponse? MessageReference,
    MessageSnapshotResponse[]? MessageSnapshots,
    string? Nonce,
    MessageCallResponse? Call
) : IIdentifiableDto
{
    [JsonPropertyName("timestamp")]
    public DateTimeOffset CreatedAt { get; init; } = CreatedAt;

    [JsonPropertyName("edited_timestamp")]
    public DateTimeOffset? EditedAt { get; init; } = EditedAt;

    [JsonPropertyName("mention_everyone")]
    public bool HasEveryoneMention { get; init; } = HasEveryoneMention;
}