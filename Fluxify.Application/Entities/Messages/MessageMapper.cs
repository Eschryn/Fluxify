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
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Fluxify.Dto.Channels.Text.Messages.Reactions;
using Fluxify.Dto.Guilds.Emoji;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Messages;

[Mapper]
[UseStaticMapper(typeof(CommonMapper))]
public partial class MessageMapper(
    FluxerApplication application
)
{
    private partial MessageAttachmentFlags MapToString(AttachmentFlags color);

    private partial MessageEmbedResponse MapToMessageEmbedResponse(Embed embed);

    [MapperIgnoreSource(nameof(Embed.Children))]
    private partial MessageEmbedChildResponse MapToMessageEmbedChildResponse(Embed embed);
    
    [MapValue(nameof(Embed.Children), null)]
    private partial Embed MapToEmbed(MessageEmbedChildResponse embed);
    
    [MapperIgnoreSource(nameof(Attachment.ContentHash))]
    [MapperIgnoreSource(nameof(Attachment.ContentType))]
    [MapperIgnoreSource(nameof(Attachment.Duration))]
    [MapperIgnoreSource(nameof(Attachment.Expired))]
    [MapperIgnoreSource(nameof(Attachment.ExpiresAt))]
    [MapperIgnoreSource(nameof(Attachment.Height))]
    [MapperIgnoreSource(nameof(Attachment.Width))]
    [MapperIgnoreSource(nameof(Attachment.Nsfw))]
    [MapperIgnoreSource(nameof(Attachment.Waveform))]
    [MapperIgnoreSource(nameof(Attachment.Placeholder))]
    [MapperIgnoreSource(nameof(Attachment.ProxyUrl))]
    [MapperIgnoreSource(nameof(Attachment.Size))]
    [MapperIgnoreSource(nameof(Attachment.Url))]
    private partial MessageAttachmentRequest Map(Attachment attachment);
    
    public partial CreateMessageRequest Map(MessageDto message);
    public async Task<Message> MapAsync(MessageResponse message, IUser author, IUser? referencedAuthor = null)  
        => Map(message, (TextChannel)await application.Channels.GetAsync(message.ChannelId), author, referencedAuthor);
    public Task<Message> MapAsync(MessageResponse message)  
        => MapAsync(message, GetAuthor(message), message.ReferencedMessage != null ? GetAuthor(message.ReferencedMessage) : null);
    
    private IUser GetAuthor(MessageBaseResponse message) 
        => (application.Channels.GetCachedOrDefault<IChannel>(message.ChannelId) is IGuildChannel guildChannel
            ? (IUser?)guildChannel.Guild.MembersRepository.Cache.GetCachedOrDefault<GuildUser>(message.Author.Id)
            : application.Users.GetCachedOrDefault(message.Author.Id))
           ?? application.Users.Insert(message.Author);
    
    [MapProperty(nameof(MessageCallResponse.EndedTimestamp), nameof(CallInfo.EndedAt))]
    private partial CallInfo MapCall(MessageCallResponse call);

    [MapperIgnoreTarget(nameof(Sticker.Tags))]
    [MapperIgnoreTarget(nameof(Sticker.Description))]
    [MapProperty(nameof(MessageStickerResponse.Animated), nameof(Sticker.IsAnimated))]
    private partial Sticker MapSticker(MessageStickerResponse sticker);
    
    [MapProperty(nameof(MessageBaseResponse.Timestamp), nameof(Message.CreatedAt))]
    [MapProperty(nameof(MessageBaseResponse.EditedTimestamp), nameof(Message.EditedAt))]
    [MapProperty(nameof(MessageBaseResponse.MentionEveryone), nameof(Message.MentionsEveryone))]
    [MapProperty(nameof(MessageBaseResponse.Tts), nameof(Message.HasTts))]
    [MapProperty(nameof(MessageBaseResponse.Pinned), nameof(Message.IsPinned))]
    [MapperIgnoreSource(nameof(MessageBaseResponse.ChannelId))]
    [MapperIgnoreSource(nameof(MessageBaseResponse.Author))]
    [MapValue(nameof(Message.ReferencedMessage), null)]
    private partial Message MapMessageBase(MessageBaseResponse message, TextChannel channel, IUser author, FluxerApplication application);
    public Message MapMessageBase(MessageBaseResponse message, TextChannel channel, IUser author) 
        => MapMessageBase(message, channel, author, application);
    
    [MapProperty(nameof(MessageSnapshotResponse.Timestamp), nameof(MessageSnapshot.CreatedAt))]
    [MapProperty(nameof(MessageSnapshotResponse.EditedTimestamp), nameof(MessageSnapshot.EditedAt))]
    private partial MessageSnapshot MapSnapshot(MessageSnapshotResponse message);
    
    public Message Map(MessageResponse message, TextChannel channel, IUser author, IUser? referencedAuthor)
        => Map(
            message,
            channel,
            author,
            message.ReferencedMessage != null 
                ? MapMessageBase(message.ReferencedMessage, channel, referencedAuthor!)
                : null,
            application);

    [MapProperty(nameof(MessageResponse.Timestamp), nameof(Message.CreatedAt))]
    [MapProperty(nameof(MessageResponse.EditedTimestamp), nameof(Message.EditedAt))]
    [MapProperty(nameof(MessageResponse.MentionEveryone), nameof(Message.MentionsEveryone))]
    [MapProperty(nameof(MessageResponse.Tts), nameof(Message.HasTts))]
    [MapProperty(nameof(MessageResponse.Pinned), nameof(Message.IsPinned))]
    [MapperIgnoreSource(nameof(MessageResponse.ChannelId))]
    [MapperIgnoreSource(nameof(MessageResponse.ReferencedMessage))]
    [MapperIgnoreSource(nameof(MessageResponse.Author))]
    private partial Message Map(MessageResponse message, TextChannel channel, IUser author, Message? referencedMessage, FluxerApplication application);
}