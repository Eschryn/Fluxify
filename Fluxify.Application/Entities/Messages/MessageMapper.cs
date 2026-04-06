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
using Fluxify.Application.State.Ref;
using Fluxify.Dto.Channels.GroupDm;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Attachments;
using Fluxify.Dto.Channels.Text.Messages.Embeds.Response;
using Fluxify.Dto.Channels.Text.Messages.Reference;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.ScheduledMessages;
using Fluxify.Dto.Webhooks;
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
    public partial CreateWebhookMessageRequest Map(MessageCreate message, string? username, string? avatarUrl);
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
    [MapperIgnoreSource(nameof(Message.Guild))]
    [MapperIgnoreSource(nameof(Message.AuthorRef))]
    [MapperIgnoreSource(nameof(Message.ChannelRef))]
    [MapperIgnoreSource(nameof(Message.ReferencedMessageRef))]
    [MapValue(nameof(MessageEdit.AllowedMentions), null)]
    public partial MessageEdit MapToEdit(Message message);

    public partial UpdateMessageRequest Map(MessageEdit message);

    public async Task<Message> MapAsync(MessageResponse message)
    {
        var channelRef = await application.ChannelsRepository.GetAsync(message.ChannelId);
        var authorRef = channelRef.Value switch
        {
            _ when message.Author.Id == message.WebhookId => new CacheRef<IUser>(
                message.Author.Id,
                application.UserMapper.MapWebhook(message.Author)
            ),
            IGuildChannel { Guild: { } guild } => (ICacheRef<IUser>)await guild.MembersRepository.GetAsync(
                message.Author.Id),
            _ when message is GatewayMessage { GuildId: { } guildId }
                => await (
                    await application.GuildsRepository.GetAsync(guildId)
                ).Value!.MembersRepository.GetAsync(message.Author.Id),
            _ => await application.UsersRepository.GetAsync(message.Author.Id)
        };

        return Map(message, channelRef, authorRef);
    }

    public Message Map(MessageResponse message, CacheRef<IChannel>? channel = null, ICacheRef<IUser>? author = null)
        => Map(
            message,
            (channel ?? new CacheRef<IChannel>(message.ChannelId, null)).Cast<ITextChannel>(),
            author ?? GetAuthor(message),
            message.ReferencedMessage != null
                ? GetAuthor(message.ReferencedMessage)
                : null
        );

    private ICacheRef<IUser> GetAuthor(MessageBaseResponse message)
        => application.ChannelsRepository.GetCachedOrDefault(message.ChannelId).Value is IGuildChannel
        {
            Guild.MembersRepository: { } membersRepository
        }
            ? membersRepository.Cache.GetCachedOrDefault(message.Author.Id)
            : application.UsersRepository.Insert(message.Author);

    [MapProperty(nameof(MessageCallResponse.EndedTimestamp), nameof(CallInfo.EndedAt))]
    private partial CallInfo MapCall(MessageCallResponse call);

    [MapperIgnoreTarget(nameof(Sticker.Tags))]
    [MapperIgnoreTarget(nameof(Sticker.Description))]
    [MapProperty(nameof(MessageStickerResponse.Animated), nameof(Sticker.IsAnimated))]
    private partial Sticker MapSticker(MessageStickerResponse sticker);

    private ICacheRef<IUser> MapUser(UserPartialResponse user) => application.UsersRepository.Insert(user);

    [MapProperty(nameof(MessageBaseResponse.Timestamp), nameof(Message.CreatedAt))]
    [MapProperty(nameof(MessageBaseResponse.EditedTimestamp), nameof(Message.EditedAt))]
    [MapProperty(nameof(MessageBaseResponse.MentionEveryone), nameof(Message.MentionsEveryone))]
    [MapProperty(nameof(MessageBaseResponse.Tts), nameof(Message.HasTts))]
    [MapProperty(nameof(MessageBaseResponse.Pinned), nameof(Message.IsPinned))]
    [MapperIgnoreSource(nameof(MessageBaseResponse.ChannelId))]
    [MapperIgnoreSource(nameof(MessageBaseResponse.Author))]
    [MapperIgnoreTarget(nameof(Message.Author))]
    [MapperIgnoreTarget(nameof(Message.Channel))]
    [MapValue(nameof(Message.ReferencedMessageRef), null)]
    private partial Message MapMessageBase(MessageBaseResponse message, ICacheRef<ITextChannel> channelRef,
        ICacheRef<IUser> authorRef, FluxerApplication application);

    private ICacheRef<Message> MapMessageBase(MessageBaseResponse message, ICacheRef<ITextChannel> channel,
        ICacheRef<IUser> author)
    {
        var repository = channel.Value switch
        {
            GuildTextChannel textChannel => textChannel.MessageRepository,
            PrivateTextChannel textChannel => textChannel.MessageRepository,
            _ => throw new InvalidOperationException("Channel type not implemented!")
        };

        return repository.Cache.GetCachedOrDefault(message.Id) is { Value: { } } messageRef
            ? messageRef
            : new CacheRef<Message>(message.Id, MapMessageBase(message, channel, author, application));
    }

    [MapProperty(nameof(MessageSnapshotResponse.Timestamp), nameof(MessageSnapshot.CreatedAt))]
    [MapProperty(nameof(MessageSnapshotResponse.EditedTimestamp), nameof(MessageSnapshot.EditedAt))]
    private partial MessageSnapshot MapSnapshot(MessageSnapshotResponse message);

    public Message Map(MessageResponse message, ICacheRef<ITextChannel> channel, ICacheRef<IUser> author,
        ICacheRef<IUser>? referencedAuthor)
        => Map(
            message,
            author,
            channel,
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
    private partial Message Map(
        MessageResponse message,
        ICacheRef<IUser> authorRef,
        ICacheRef<ITextChannel> channelRef,
        ICacheRef<Message>? referencedMessageRef,
        FluxerApplication application
    );

    [MapperIgnoreSource(nameof(Message.Id))]
    [MapperIgnoreSource(nameof(Message.Guild))]
    [MapperIgnoreSource(nameof(Message.AuthorRef))]
    [MapperIgnoreSource(nameof(Message.ChannelRef))]
    public partial void UpdateEntity([MappingTarget] Message data, Message update);
}