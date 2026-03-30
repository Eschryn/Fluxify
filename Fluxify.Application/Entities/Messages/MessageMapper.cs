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
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds.Response;
using Fluxify.Dto.Channels.Text.Messages.Reference;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.ScheduledMessages;
using Fluxify.Gateway.Model.Data.Channel.Message;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Messages;

[Mapper]
[UseStaticMapper(typeof(CommonMapper))]
public partial class MessageMapper(
    FluxerApplication application
) : IUpdateEntity<Message>
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
    
    public partial CreateMessageRequest Map(MessageCreate message);
    public partial ScheduledMessageSchema Map(MessageCreate messageCreate, DateTime scheduledLocalAt, string timezone);
    
    [MapperIgnoreSource(nameof(Message.Id))] 
    [MapperIgnoreSource(nameof(Message.Reactions))] 
    [MapperIgnoreSource(nameof(Message.Author))] 
    [MapperIgnoreSource(nameof(Message.Channel))] 
    [MapperIgnoreSource(nameof(Message.EditedAt))] 
    [MapperIgnoreSource(nameof(Message.CreatedAt))] 
    [MapperIgnoreSource(nameof(Message.Call))] 
    [MapperIgnoreSource(nameof(Message.MentionRoles))] 
    [MapperIgnoreSource(nameof(Message.MentionedRoles))] 
    [MapperIgnoreSource(nameof(Message.Mentions))] 
    [MapperIgnoreSource(nameof(Message.MentionsEveryone))] 
    [MapperIgnoreSource(nameof(Message.MessageReference))] 
    [MapperIgnoreSource(nameof(Message.MessageSnapshots))] 
    [MapperIgnoreSource(nameof(Message.ReferencedMessage))] 
    [MapperIgnoreSource(nameof(Message.Stickers))] 
    [MapperIgnoreSource(nameof(Message.WebhookId))] 
    [MapperIgnoreSource(nameof(Message.HasTts))] 
    [MapperIgnoreSource(nameof(Message.IsPinned))] 
    [MapperIgnoreSource(nameof(Message.Nonce))] 
    [MapperIgnoreSource(nameof(Message.Type))] 
    [MapValue(nameof(MessageEdit.AllowedMentions), null)]
    public partial MessageEdit MapToEdit(Message message);
    public partial UpdateMessageRequest Map(MessageEdit message);
    
    public async Task<Message> MapAsync(MessageResponse message, ITextChannel? channel = null, IUser? author = null)
    {
        channel ??= (ITextChannel)await application.ChannelsRepository.GetAsync(message.ChannelId);
        author ??= channel switch
        {
            IGuildChannel guildChannel => await guildChannel.Guild.MembersRepository.GetAsync(message.Author.Id),
            _ when message is GatewayMessage { GuildId: {} guildId } 
                => await (
                    await application.GuildsRepository.GetAsync(guildId)
                ).MembersRepository.GetAsync(message.Author.Id),
            _ => await application.UsersRepository.GetAsync(message.Author.Id)
        };
        
        return Map(message, channel, author);
    }

    public Message Map(MessageResponse message, ITextChannel? channel = null, IUser? author = null)    
        => Map(
            message,
            channel ?? CreateMinimalChannel(message),
            author ?? GetAuthor(message),
            message.ReferencedMessage != null 
                ? GetAuthor(message.ReferencedMessage) 
                : null
        );
    
    
    private ITextChannel CreateMinimalChannel(MessageResponse response)
    {
        var guild = response is GatewayMessage { GuildId: { } guildId }
            ? application.GuildsRepository.Cache.GetCachedOrDefault<Guild>(guildId) ?? new Guild(application)
            {
                Id = guildId,
                Name = string.Empty,
                Owner = new GlobalUser()
            }
            : null;
        
        return guild != null ? 
            new GuildTextChannel(application)
            {
                Id = response.ChannelId,
                Guild = guild,
                Name = string.Empty,
                Parent = null
            }
            : new Dm(application)
            {
                Id = response.ChannelId
            };
    }
    
    private IUser GetAuthor(MessageBaseResponse message) 
        => (application.ChannelsRepository.GetCachedOrDefault<IChannel>(message.ChannelId) is IGuildChannel guildChannel
            ? (IUser?)guildChannel.Guild.MembersRepository.Cache.GetCachedOrDefault<GuildUser>(message.Author.Id)
            : application.UsersRepository.GetCachedOrDefault(message.Author.Id))
           ?? application.UsersRepository.Insert(message.Author);
    
    [MapProperty(nameof(MessageCallResponse.EndedTimestamp), nameof(CallInfo.EndedAt))]
    private partial CallInfo MapCall(MessageCallResponse call);

    [MapperIgnoreTarget(nameof(Sticker.Tags))]
    [MapperIgnoreTarget(nameof(Sticker.Description))]
    [MapProperty(nameof(MessageStickerResponse.Animated), nameof(Sticker.IsAnimated))]
    private partial Sticker MapSticker(MessageStickerResponse sticker);

    private IUser MapUser(UserPartialResponse user) => application.UsersRepository.GetCachedOrDefault(user.Id) ?? application.UsersRepository.Insert(user);
    
    [MapProperty(nameof(MessageBaseResponse.Timestamp), nameof(Message.CreatedAt))]
    [MapProperty(nameof(MessageBaseResponse.EditedTimestamp), nameof(Message.EditedAt))]
    [MapProperty(nameof(MessageBaseResponse.MentionEveryone), nameof(Message.MentionsEveryone))]
    [MapProperty(nameof(MessageBaseResponse.Tts), nameof(Message.HasTts))]
    [MapProperty(nameof(MessageBaseResponse.Pinned), nameof(Message.IsPinned))]
    [MapperIgnoreSource(nameof(MessageBaseResponse.ChannelId))]
    [MapperIgnoreSource(nameof(MessageBaseResponse.Author))]
    [MapValue(nameof(Message.ReferencedMessage), null)]
    private partial Message MapMessageBase(MessageBaseResponse message, ITextChannel channel, IUser author, FluxerApplication application);
    public Message MapMessageBase(MessageBaseResponse message, ITextChannel channel, IUser author) 
        => MapMessageBase(message, channel, author, application);
    
    [MapProperty(nameof(MessageSnapshotResponse.Timestamp), nameof(MessageSnapshot.CreatedAt))]
    [MapProperty(nameof(MessageSnapshotResponse.EditedTimestamp), nameof(MessageSnapshot.EditedAt))]
    private partial MessageSnapshot MapSnapshot(MessageSnapshotResponse message);
    
    public Message Map(MessageResponse message, ITextChannel channel, IUser author, IUser? referencedAuthor)
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
    private partial Message Map(MessageResponse message, ITextChannel channel, IUser author, Message? referencedMessage, FluxerApplication application);

    [MapperIgnoreSource(nameof(Message.Id))]
    public partial void UpdateEntity([MappingTarget] Message data, Message update);
}