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

using System.Runtime.CompilerServices;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Entities.Guilds.Members;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Application.State;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Embeds.Response;
using Fluxify.Dto.Channels.Text.Messages.Reference;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.ScheduledMessages;
using Fluxify.Dto.Webhooks;
using Fluxify.Gateway.Model.Data.Channel.Message;

namespace Fluxify.Application.Entities.Messages;

[Mapper]
[UseStaticMapper(typeof(CommonMapper))]
public partial class MessageMapper(
    FluxerApplication application
) : IUpdateEntity<Message, MessageResponse>, ICreateEntity<Message, MessageResponse>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected FluxerApplication GetApplication() => application;

    [NamedMapping("MapMessageFromResponse"),
     IncludeMappingConfiguration(nameof(MapMessageBase)),
     MapperIgnoreSource(nameof(MessageResponse.ReferencedMessage))]
    public partial Message MapFromResponse(MessageResponse message);
    
    [IncludeMappingConfiguration("MapMessageFromResponse")]
    public partial void UpdateEntity([MappingTarget] Message data, MessageResponse update);

    [MapValue(nameof(Embed.Children), null)]
    private partial Embed MapToEmbed(MessageEmbedChildResponse embed);

    [MapperIgnoreSource(nameof(Embed.Children))]
    private partial MessageEmbedChildResponse MapToMessageEmbedChildResponse(Embed embed);

    private partial MessageSnapshot MapSnapshot(MessageSnapshotResponse message);

    [MapperIgnoreSource(nameof(MessageBaseResponse.Id)),
     MapperIgnoreTarget(nameof(MessageBaseResponse.Id)),
     MapperIgnoreSource(nameof(MessageBaseResponse.Author)),
     MapperIgnoreSource(nameof(MessageBaseResponse.ChannelId)),
     MapPropertyFromSource(nameof(Message.ReferencedMessageRef), Use = nameof(ResolveMention))]
    private partial Message MapMessageBase(MessageBaseResponse message);

    [MapperRequiredMapping(RequiredMappingStrategy.Target),
     MapValue(nameof(MessageEdit.AllowedMentions), null)]
    public partial MessageEdit MapToEdit(Message message);

    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public partial AttachmentProperties MapToProperties(Attachment message);

    public partial UpdateMessageRequest MapToRequest(MessageEdit message);

    public partial CreateMessageRequest MapToRequest(MessageCreate message);
    public partial CreateWebhookMessageRequest MapToRequest(MessageCreate message, string? username, string? avatarUrl);

    public partial ScheduledMessageSchema MapToRequest(MessageCreate messageCreate, DateTime scheduledLocalAt,
        string timezone);

    private ICacheRef<Message>? ResolveMention(MessageBaseResponse reference)
    {
        if (reference.MessageReference is null)
            return null;

        return application.ChannelsRepository.GetCachedOrDefault(reference.MessageReference.ChannelId).Value switch
        {
            GuildTextChannel guildTextChannel => guildTextChannel.MessageRepository.Cache.GetCachedOrDefault(
                reference.MessageReference.MessageId),
            PrivateTextChannel privateTextChannel => privateTextChannel.MessageRepository.Cache.GetCachedOrDefault(
                reference.MessageReference.MessageId),
            _ => throw new InvalidOperationException()
        };
    }

    private Snowflake InsertUser(UserPartialResponse response)
        => application.UsersRepository.Insert(response).Id;

    [ObjectFactory]
    private Message CreateMessage<TMessage>(TMessage message) where TMessage : MessageBaseResponse
    {
        var channel = application.ChannelsRepository.GetCachedOrDefault(message.ChannelId);
        var authorRef = GetAuthor(message, channel);

        return new Message(application, authorRef, channel.Cast<ITextChannel>());
    }

    private ICacheRef<IUser> GetAuthor<TMessage>(TMessage message, CacheRef<IChannel> channel) where TMessage : MessageBaseResponse
    {
        if (message.WebhookId is not null)
        {
            return new CacheRef<WebhookUser>(
                message.Author.Id,
                application.UserMapper.MapWebhook(message.Author)
            );
        }
        
        var guild = (message, channel.Value) switch
        {
            (_, IGuildChannel { Guild.Id: { } id })
                when application.GuildsRepository.Cache.GetCachedOrDefault(id) is { Value: not null } guildRef
                => guildRef,
            (GatewayMessage { GuildId: { } guildId }, _)
                when application.GuildsRepository.Cache.GetCachedOrDefault(guildId) is { Value: not null } guildRef
                => guildRef,
            _ => null
        };
        
        var repository = guild?.Value?.MembersRepository;
        return (repository, message) switch
        {
            ({} members, GatewayMessage { Author: { } author, Member: {} member })
                => members.Cache.UpdateOrCreate(author.Id, new MemberInsert(
                    member,
                    application.UsersRepository.Insert(author),
                    guild!)),
            (null, { Author: { } author })
                => application.UsersRepository.Insert(author),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}