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
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Channels.Text.Messages.Scheduled;
using Fluxify.Dto.Uploads;

namespace Fluxify.Dto.Users.ScheduledMessages;

public record UpdateScheduledMessageRequest(
    Snowflake Id,
    AllowedMentionsSchema? AllowedMentions,
    MessageAttachmentResponse[]? Attachments,
    FileUpload[]? Files,
    string? Content,
    MessageEmbedResponse[]? Embeds,
    Snowflake? FavoriteMemeId,
    MessageFlags? Flags,
    ScheduledMessageReferenceSchema MessageReference,
    Snowflake? Nonce,
    Snowflake[]? StickerIds,
    MessageStickerResponse[]? Stickers,
    bool? Tts
) : ScheduledMessageResponseSchemaPayload(
    AllowedMentions,
    Attachments,
    Files,
    Content,
    Embeds,
    FavoriteMemeId,
    Flags,
    MessageReference,
    Nonce,
    StickerIds,
    Stickers,
    Tts
);