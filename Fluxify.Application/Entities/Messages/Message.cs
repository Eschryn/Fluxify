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
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Roles;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Guild;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Application.State;
using Fluxify.Rest.Channel.Messages;

namespace Fluxify.Application.Entities.Messages;

public partial class Message : IEntity, ICloneable<Message>
{
    private readonly FluxerApplication _application;
    
    private MessageRequestBuilder RequestBuilder => field ??= _application.Rest.Channels[Channel.Id].Messages[Id];
    private ICacheRef<IUser> AuthorRef { get; }
    private ICacheRef<ITextChannel> ChannelRef { get; }
    
    public Snowflake Id { get; init; }
    public string? Content { get; internal set; }

    public Snowflake? WebhookId { get; internal set; }
    public IUser Author => field = AuthorRef.Value ?? field;
    public ITextChannel Channel => field = ChannelRef.Value ?? field;
    public Guild? Guild => Channel is IGuildChannel guildChannel ? guildChannel.Guild : null;
    public Attachment[]? Attachments { get; internal set; }
    public Embed[]? Embeds { get; internal set; }
    public bool HasEveryoneMention { get; internal set; }
    public Reaction[]? Reactions { get; internal set; }
    public Sticker[]? Stickers { get; internal set; }

    public DateTimeOffset CreatedAt { get; internal set; }
    public DateTimeOffset? EditedAt { get; internal set; }

    public string? Nonce { get; internal set; }

    public MessageFlags Flags { get; internal set; }
    public MessageType Type { get; internal set; }

    public bool IsPinned { get; internal set; }
    public bool? HasTts { get; internal set; }

    internal Snowflake[]? Mentions { get; set; }
    internal Snowflake[] MentionRoles { get; set; } = [];
    
    [MapperIgnore]
    public IEnumerable<IUser> MentionedUsers 
        => Mentions?.Select(ResolveUser).OfType<IUser>() ?? [];
    
    [MapperIgnore]
    public IRole[] MentionedRoles => Channel switch
    {
        IGuildChannel guildChannel
            => MentionRoles
                .Select(
                    id => guildChannel.Guild?.RolesRepository.Cache.GetCachedOrDefault(id).Value)
                .OfType<IRole>()
                .ToArray(),
        _ => []
    };

    public CallInfo? Call { get; internal set; }

    public MessageReference? MessageReference { get; internal set; }
    public MessageSnapshot[]? MessageSnapshots { get; internal set; }
    internal ICacheRef<Message>? ReferencedMessageRef { get; set; }
    public Message? ReferencedMessage => MessageReference != null ? ReferencedMessageRef?.Value : null;

    internal Message(
        FluxerApplication application,
        ICacheRef<IUser> authorRef,
        ICacheRef<ITextChannel> channelRef
    )
    {
        _application = application;
        AuthorRef = authorRef;
        ChannelRef = channelRef;
        
        Author = authorRef.Value!;
        Channel = channelRef.Value!;
    }

    private IUser? ResolveUser(Snowflake id)
    {
        if (Channel is IGuildChannel guildChannel
            && guildChannel.Guild?.MembersRepository.Cache.GetCachedOrDefault(id) is { Value: { } value })
        { 
            return value;
        }
        
        return _application.UsersRepository.Cache.GetCachedOrDefault(id).Value;
    }
    
    public object Clone() => MemberwiseClone();
}