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
using Fluxify.Dto.Channels.Text.Messages.Embeds.Response;
using Fluxify.Dto.Channels.Text.Messages.Reference;
using Fluxify.Dto.Uploads;

namespace Fluxify.Dto.Users.ScheduledMessages;

public record ScheduledMessageSchema(
    DateTime ScheduledLocalAt,
    string Timezone,
    string? Content = null,
    MessageAttachmentRequest[]? Attachments = null,
    MessageEmbedResponse[]? Embeds = null,
    AllowedMentionsSchema? AllowedMentions = null,
    MessageReferenceResponse? MessageReference = null,
    MessageFlags? Flags = null,
    string? Nonce = null,
    Snowflake? FavoriteMemeId = null,
    Snowflake[]? Stickers = null,
    bool? Tts = null,
    FileUpload[]? Files = null
) : CreateMessageRequest(
    Content,
    Attachments,
    Embeds,
    AllowedMentions,
    MessageReference,
    Flags,
    Nonce,
    FavoriteMemeId,
    Stickers,
    Tts,
    Files
);