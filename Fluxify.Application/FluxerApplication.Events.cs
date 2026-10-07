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

using Fluxify.Application.Common;
using Fluxify.Application.EventArgs;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    private readonly AsyncEventHandlerContainer<MessageEventArgs> _messageCreateHandlers = new();
    private readonly AsyncEventHandlerContainer<MessageEventArgs> _messageUpdateHandlers = new();
    private readonly AsyncEventHandlerContainer<MessageDeletedEventArgs> _messageDeleteHandlers = new();
    private readonly AsyncEventHandlerContainer<MessagesBulkDeletedEventArgs> _messageBulkDeletedHandlers = new();

    private readonly AsyncEventHandlerContainer<ReactionEventArgs> _messageReactionAddHandlers = new();
    private readonly AsyncEventHandlerContainer<ReactionEventArgs> _messageReactionRemoveHandlers = new();
    private readonly AsyncEventHandlerContainer<ReactionRemoveEmojiEventArgs> _messageReactionRemoveEmojiHandlers = new();
    private readonly AsyncEventHandlerContainer<ReactionRemoveAllEventArgs> _messageReactionRemoveAllHandlers = new();

    private readonly AsyncEventHandlerContainer<ChannelEventArgs> _channelCreatedHandlers = new();
    private readonly AsyncEventHandlerContainer<ChannelUpdatedEventArgs> _channelUpdatedHandlers = new();
    private readonly AsyncEventHandlerContainer<ChannelEventArgs> _channelDeletedHandlers = new();

    private readonly AsyncEventHandlerContainer<GuildEventArgs> _guildCreatedHandlers = new();
    private readonly AsyncEventHandlerContainer<GuildEventArgs> _guildUpdatedHandlers = new();
    private readonly AsyncEventHandlerContainer<GuildDeletedEventArgs> _guildDeletedHandlers = new();

    private readonly AsyncEventHandlerContainer<GuildRoleEventArgs> _roleCreateHandlers = new();
    private readonly AsyncEventHandlerContainer<GuildRoleEventArgs> _roleUpdateHandlers = new();
    private readonly AsyncEventHandlerContainer<GuildRoleEventArgs> _roleDeletedHandlers = new();
    private readonly AsyncEventHandlerContainer<GuildRolesEventArgs> _roleUpdateBulkHandlers = new();

    private readonly AsyncEventHandlerContainer<GuildMemberEventArgs> _guildMemberAddedHandlers = new();
    private readonly AsyncEventHandlerContainer<GuildMemberEventArgs> _guildMemberUpdatedHandlers = new();
    private readonly AsyncEventHandlerContainer<GuildMemberEventArgs> _guildMemberRemovedHandlers = new();
    
    private readonly AsyncEventHandlerContainer<GuildBanEventArgs> _guildBanAddHandlers = new();
    private readonly AsyncEventHandlerContainer<GuildBanEventArgs> _guildBanRemoveHandlers = new();
    private readonly AsyncEventHandlerContainer<GroupMembershipEventArgs> _groupMemberAddedHandlers = new();
    private readonly AsyncEventHandlerContainer<GroupMembershipEventArgs> _groupMemberRemovedHandlers = new();

    /// <inheritdoc />
    public event AsyncEventHandler<MessageEventArgs> MessageReceived
    {
        add => _messageCreateHandlers.InsertDelegate(value);
        remove => _messageCreateHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<MessageEventArgs> MessageUpdated
    {
        add => _messageUpdateHandlers.InsertDelegate(value);
        remove => _messageUpdateHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<MessageDeletedEventArgs> MessageDeleted
    {
        add => _messageDeleteHandlers.InsertDelegate(value);
        remove => _messageDeleteHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<MessagesBulkDeletedEventArgs> MessageBulkDeleted
    {
        add => _messageBulkDeletedHandlers.InsertDelegate(value);
        remove => _messageBulkDeletedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<ReactionEventArgs> MessageReactionAdd
    {
        add => _messageReactionAddHandlers.InsertDelegate(value);
        remove => _messageReactionAddHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<ReactionEventArgs> MessageReactionRemove
    {
        add => _messageReactionRemoveHandlers.InsertDelegate(value);
        remove => _messageReactionRemoveHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<ReactionRemoveEmojiEventArgs> MessageReactionRemoveEmoji
    {
        add => _messageReactionRemoveEmojiHandlers.InsertDelegate(value);
        remove => _messageReactionRemoveEmojiHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<ReactionRemoveAllEventArgs> MessageReactionRemoveAll
    {
        add => _messageReactionRemoveAllHandlers.InsertDelegate(value);
        remove => _messageReactionRemoveAllHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildEventArgs> GuildCreated
    {
        add => _guildCreatedHandlers.InsertDelegate(value);
        remove => _guildCreatedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildEventArgs> GuildUpdated
    {
        add => _guildUpdatedHandlers.InsertDelegate(value);
        remove => _guildUpdatedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildDeletedEventArgs> GuildDeleted
    {
        add => _guildDeletedHandlers.InsertDelegate(value);
        remove => _guildDeletedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<ChannelEventArgs> ChannelCreated
    {
        add => _channelCreatedHandlers.InsertDelegate(value);
        remove => _channelCreatedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<ChannelUpdatedEventArgs> ChannelUpdated
    {
        add => _channelUpdatedHandlers.InsertDelegate(value);
        remove => _channelUpdatedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<ChannelEventArgs> ChannelDeleted
    {
        add => _channelDeletedHandlers.InsertDelegate(value);
        remove => _channelDeletedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildRoleEventArgs> RoleCreated
    {
        add => _roleCreateHandlers.InsertDelegate(value);
        remove => _roleCreateHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildRoleEventArgs> RoleUpdated
    {
        add => _roleUpdateHandlers.InsertDelegate(value);
        remove => _roleUpdateHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildRoleEventArgs> RoleDeleted
    {
        add => _roleDeletedHandlers.InsertDelegate(value);
        remove => _roleDeletedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildRolesEventArgs> RoleUpdatedBulk
    {
        add => _roleUpdateBulkHandlers.InsertDelegate(value);
        remove => _roleUpdateBulkHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildMemberEventArgs> GuildMemberAdded
    {
        add => _guildMemberAddedHandlers.InsertDelegate(value);
        remove => _guildMemberAddedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildMemberEventArgs> GuildMemberUpdated
    {
        add => _guildMemberUpdatedHandlers.InsertDelegate(value);
        remove => _guildMemberUpdatedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildMemberEventArgs> GuildMemberRemoved
    {
        add => _guildMemberRemovedHandlers.InsertDelegate(value);
        remove => _guildMemberRemovedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GuildBanEventArgs> GuildBanAdd
    {
        add => _guildBanAddHandlers.InsertDelegate(value);
        remove => _guildBanAddHandlers.RemoveDelegate(value);
    }
    
    /// <inheritdoc />
    public event AsyncEventHandler<GuildBanEventArgs> GuildBanRemove
    {
        add => _guildBanRemoveHandlers.InsertDelegate(value);
        remove => _guildBanRemoveHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GroupMembershipEventArgs> GroupMemberRemoved
    {
        add => _groupMemberRemovedHandlers.InsertDelegate(value);
        remove => _groupMemberRemovedHandlers.RemoveDelegate(value);
    }

    /// <inheritdoc />
    public event AsyncEventHandler<GroupMembershipEventArgs> GroupMemberAdded
    {
        add => _groupMemberAddedHandlers.InsertDelegate(value);
        remove => _groupMemberAddedHandlers.RemoveDelegate(value);
    }
}