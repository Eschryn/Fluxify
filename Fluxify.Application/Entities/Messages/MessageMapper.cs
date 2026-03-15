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

using System.Drawing;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Messages;

[Mapper]
public partial class MessageMapper(
    FluxerApplication application
)
{
    public int MapToInt(Color color) => color.ToArgb() & 0x00FFFFFF;
    public Color MapToColor(int color) => Color.FromArgb((int)((color & 0x00FFFFFF) | 0xFF000000));
    public partial MessageAttachmentFlags MapToString(AttachmentFlags color);

    [MapValue(nameof(@MessageEmbedResponse.Children), null)]
    public partial MessageEmbedResponse MapToMessageEmbedResponse(Embed embed);
    public partial CreateMessageRequest Map(MessageDto message);
    public async Task<Message> MapAsync(MessageResponse message) 
        => Map(message, (TextChannel)await application.Channels.GetAsync(message.ChannelId));
    
    [MapProperty(nameof(MessageResponse.Timestamp), nameof(Message.CreatedAt))]
    [MapProperty(nameof(MessageResponse.EditedTimestamp), nameof(Message.EditedAt))]
    [MapProperty(nameof(MessageResponse.MentionEveryone), nameof(Message.MentionsEveryone))]
    [MapProperty(nameof(MessageResponse.Tts), nameof(Message.HasTts))]
    [MapProperty(nameof(MessageResponse.Pinned), nameof(Message.IsPinned))]
    public partial Message Map(MessageResponse message, TextChannel channel);
}