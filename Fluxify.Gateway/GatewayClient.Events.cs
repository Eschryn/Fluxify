using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using Fluxify.Core;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.SavedMedia;
using Fluxify.Dto.Users;
using Fluxify.Dto.Users.GuildSettings;
using Fluxify.Dto.Users.Relationships;
using Fluxify.Dto.Users.Settings;
using Fluxify.Gateway.Model;
using Fluxify.Gateway.Model.Data;
using Fluxify.Gateway.Model.Data.Channel;
using Fluxify.Gateway.Model.Data.Guild;
using Fluxify.Gateway.Model.Dto;

namespace Fluxify.Gateway;

public sealed partial class GatewayClient
{
    private readonly FrozenDictionary<string, IHandlerContainer> _eventHandlers = 
        EventNamePayloadClassMap.HandlerContainerConstructorTable.ToFrozenDictionary(k => k.Key, v => v.Value());
    
    public event Action<ConnectionState>? ConnectionStateChanged;
    
    public event Func<ReadyPayload, Task>? Ready
    {
        add => InsertHandler(GatewayEvent.Ready, value!);
        remove => RemoveHandler(GatewayEvent.Ready, value!);
    }
    
    public event Func<Task>? Resumed
    {
        add => GetHandlerContainer(GatewayEvent.Resumed).InsertDelegate(value!);
        remove => GetHandlerContainer(GatewayEvent.Resumed).RemoveDelegate(value!);
    }
    
    public event Func<GatewaySession[], Task>? SessionsReplace
    {
        add => InsertHandler(GatewayEvent.SessionsReplace, value!);
        remove => RemoveHandler(GatewayEvent.SessionsReplace, value!);
    }
    
    public event Func<UserPrivate, Task>? UserUpdate
    {
        add => InsertHandler(GatewayEvent.UserUpdate, value!);
        remove => RemoveHandler(GatewayEvent.UserUpdate, value!);
    }
    
    public event Func<Snowflake[], Task>? UserPinnedDmsUpdate
    {
        add => InsertHandler(GatewayEvent.UserPinnedDmsUpdate, value!);
        remove => RemoveHandler(GatewayEvent.UserPinnedDmsUpdate, value!);
    }
    
    public event Func<UserSettings, Task>? UserSettingsUpdate
    {
        add => InsertHandler(GatewayEvent.UserSettingsUpdate, value!);
        remove => RemoveHandler(GatewayEvent.UserSettingsUpdate, value!);
    }
    
    public event Func<UserGuildSettings, Task>? UserGuildSettingsUpdate
    {
        add => InsertHandler(GatewayEvent.UserGuildSettingsUpdate, value!);
        remove => RemoveHandler(GatewayEvent.UserGuildSettingsUpdate, value!);
    }
    
    public event Func<GatewayUserNoteUpdate, Task>? UserNoteUpdate
    {
        add => InsertHandler(GatewayEvent.UserNoteUpdate, value!);
        remove => RemoveHandler(GatewayEvent.UserNoteUpdate, value!);
    }
    
    public event Func<GatewayMessageIdResponse, Task>? RecentMentionDelete
    {
        add => InsertHandler(GatewayEvent.RecentMentionDelete, value!);
        remove => RemoveHandler(GatewayEvent.RecentMentionDelete, value!);
    }
    
    public event Func<Message, Task>? SavedMessageCreate
    {
        add => InsertHandler(GatewayEvent.SavedMessageCreate, value!);
        remove => RemoveHandler(GatewayEvent.SavedMessageCreate, value!);
    }
    
    public event Func<GatewayMessageIdResponse, Task>? SavedMessageDelete
    {
        add => InsertHandler(GatewayEvent.SavedMessageDelete, value!);
        remove => RemoveHandler(GatewayEvent.SavedMessageDelete, value!);
    }
    
    public event Func<FavoriteMemeResponse, Task>? FavoriteMemeCreate
    {
        add => InsertHandler(GatewayEvent.FavoriteMemeCreate, value!);
        remove => RemoveHandler(GatewayEvent.FavoriteMemeCreate, value!);
    }
    
    public event Func<FavoriteMemeResponse, Task>? FavoriteMemeUpdate
    {
        add => InsertHandler(GatewayEvent.FavoriteMemeUpdate, value!);
        remove => RemoveHandler(GatewayEvent.FavoriteMemeUpdate, value!);
    }
    
    public event Func<GatewayMemeIdResponse, Task>? FavoriteMemeDelete
    {
        add => InsertHandler(GatewayEvent.FavoriteMemeDelete, value!);
        remove => RemoveHandler(GatewayEvent.FavoriteMemeDelete, value!);
    }
    
    public event Func<GatewayAuthSessionChange, Task>? AuthSessionChange
    {
        add => InsertHandler(GatewayEvent.AuthSessionChange, value!);
        remove => RemoveHandler(GatewayEvent.AuthSessionChange, value!);
    }
    
    public event Func<Presence, Task>? PresenceUpdate
    {
        add => InsertHandler(GatewayEvent.PresenceUpdate, value!);
        remove => RemoveHandler(GatewayEvent.PresenceUpdate, value!);
    }
    
    public event Func<GatewayGuildCreate, Task>? GuildCreate
    {
        add => InsertHandler(GatewayEvent.GuildCreate, value!);
        remove => RemoveHandler(GatewayEvent.GuildCreate, value!);
    }
    
    public event Func<GuildResponse, Task>? GuildUpdate
    {
        add => InsertHandler(GatewayEvent.GuildUpdate, value!);
        remove => RemoveHandler(GatewayEvent.GuildUpdate, value!);
    }
    
    public event Func<GatewayGuildDelete, Task>? GuildDelete
    {
        add => InsertHandler(GatewayEvent.GuildDelete, value!);
        remove => RemoveHandler(GatewayEvent.GuildDelete, value!);
    }
    
    public event Func<GuildMember, Task>? GuildMemberAdd
    {
        add => InsertHandler(GatewayEvent.GuildMemberAdd, value!);
        remove => RemoveHandler(GatewayEvent.GuildMemberAdd, value!);
    }
    
    public event Func<GuildMember, Task>? GuildMemberUpdate
    {
        add => InsertHandler(GatewayEvent.GuildMemberUpdate, value!);
        remove => RemoveHandler(GatewayEvent.GuildMemberUpdate, value!);
    }
    
    public event Func<GatewayGuildMemberDelete, Task>? GuildMemberRemove
    {
        add => InsertHandler(GatewayEvent.GuildMemberRemove, value!);
        remove => RemoveHandler(GatewayEvent.GuildMemberRemove, value!);
    }
    
    public event Func<GatewayGuildRole, Task>? GuildRoleCreate
    {
        add => InsertHandler(GatewayEvent.GuildRoleCreate, value!);
        remove => RemoveHandler(GatewayEvent.GuildRoleCreate, value!);
    }
    
    public event Func<GatewayGuildRole, Task>? GuildRoleUpdate
    {
        add => InsertHandler(GatewayEvent.GuildRoleUpdate, value!);
        remove => RemoveHandler(GatewayEvent.GuildRoleUpdate, value!);
    }
    
    public event Func<GatewayGuildRoleDelete, Task>? GuildRoleDelete
    {
        add => InsertHandler(GatewayEvent.GuildRoleDelete, value!);
        remove => RemoveHandler(GatewayEvent.GuildRoleDelete, value!);
    }
    
    public event Func<GatewayGuildRoleBulk, Task>? GuildRoleUpdateBulk
    {
        add => InsertHandler(GatewayEvent.GuildRoleUpdateBulk, value!);
        remove => RemoveHandler(GatewayEvent.GuildRoleUpdateBulk, value!);
    }
    
    public event Func<GatewayEmojiUpdate, Task>? GuildEmojisUpdate
    {
        add => InsertHandler(GatewayEvent.GuildEmojisUpdate, value!);
        remove => RemoveHandler(GatewayEvent.GuildEmojisUpdate, value!);
    }
    
    public event Func<GatewayStickerUpdate, Task>? GuildStickersUpdate
    {
        add => InsertHandler(GatewayEvent.GuildStickersUpdate, value!);
        remove => RemoveHandler(GatewayEvent.GuildStickersUpdate, value!);
    }
    
    public event Func<GatewayBanData, Task>? GuildBanAdd
    {
        add => InsertHandler(GatewayEvent.GuildBanAdd, value!);
        remove => RemoveHandler(GatewayEvent.GuildBanAdd, value!);
    }
    
    public event Func<GatewayBanData, Task>? GuildBanRemove
    {
        add => InsertHandler(GatewayEvent.GuildBanRemove, value!);
        remove => RemoveHandler(GatewayEvent.GuildBanRemove, value!);
    }
    
    public event Func<Channel, Task>? ChannelCreate
    {
        add => InsertHandler(GatewayEvent.ChannelCreate, value!);
        remove => RemoveHandler(GatewayEvent.ChannelCreate, value!);
    }
    
    public event Func<Channel, Task>? ChannelUpdate
    {
        add => InsertHandler(GatewayEvent.ChannelUpdate, value!);
        remove => RemoveHandler(GatewayEvent.ChannelUpdate, value!);
    }
    
    public event Func<GatewayBulkChannelUpdate, Task>? ChannelUpdateBulk
    {
        add => InsertHandler(GatewayEvent.ChannelUpdateBulk, value!);
        remove => RemoveHandler(GatewayEvent.ChannelUpdateBulk, value!);
    }
    
    public event Func<Channel, Task>? ChannelDelete
    {
        add => InsertHandler(GatewayEvent.ChannelDelete, value!);
        remove => RemoveHandler(GatewayEvent.ChannelDelete, value!);
    }
    
    public event Func<GatewayChannelPinsUpdate, Task>? ChannelPinsUpdate
    {
        add => InsertHandler(GatewayEvent.ChannelPinsUpdate, value!);
        remove => RemoveHandler(GatewayEvent.ChannelPinsUpdate, value!);
    }
    
    public event Func<GatewayChannelPinsAck, Task>? ChannelPinsAck
    {
        add => InsertHandler(GatewayEvent.ChannelPinsAck, value!);
        remove => RemoveHandler(GatewayEvent.ChannelPinsAck, value!);
    }
    
    public event Func<GatewayGroupChange, Task>? ChannelRecipientAdd
    {
        add => InsertHandler(GatewayEvent.ChannelRecipientAdd, value!);
        remove => RemoveHandler(GatewayEvent.ChannelRecipientAdd, value!);
    }
    
    public event Func<GatewayGroupChange, Task>? ChannelRecipientRemove
    {
        add => InsertHandler(GatewayEvent.ChannelRecipientRemove, value!);
        remove => RemoveHandler(GatewayEvent.ChannelRecipientRemove, value!);
    }
    
    public event Func<GatewayMessageCreate, Task>? MessageCreate
    {
        add => InsertHandler(GatewayEvent.MessageCreate, value!);
        remove => RemoveHandler(GatewayEvent.MessageCreate, value!);
    }
    
    public event Func<Message, Task>? MessageUpdate
    {
        add => InsertHandler(GatewayEvent.MessageUpdate, value!);
        remove => RemoveHandler(GatewayEvent.MessageUpdate, value!);
    }
    
    public event Func<GatewayMessageDelete, Task>? MessageDelete
    {
        add => InsertHandler(GatewayEvent.MessageDelete, value!);
        remove => RemoveHandler(GatewayEvent.MessageDelete, value!);
    }
    
    public event Func<GatewayMessageDeleteBulk, Task>? MessageDeleteBulk
    {
        add => InsertHandler(GatewayEvent.MessageDeleteBulk, value!);
        remove => RemoveHandler(GatewayEvent.MessageDeleteBulk, value!);
    }
    
    public event Func<GatewayReaction, Task>? MessageReactionAdd
    {
        add => InsertHandler(GatewayEvent.MessageReactionAdd, value!);
        remove => RemoveHandler(GatewayEvent.MessageReactionAdd, value!);
    }
    
    public event Func<GatewayReaction, Task>? MessageReactionRemove
    {
        add => InsertHandler(GatewayEvent.MessageReactionRemove, value!);
        remove => RemoveHandler(GatewayEvent.MessageReactionRemove, value!);
    }
    
    public event Func<GatewayReactionRemoveAll, Task>? MessageReactionRemoveAll
    {
        add => InsertHandler(GatewayEvent.MessageReactionRemoveAll, value!);
        remove => RemoveHandler(GatewayEvent.MessageReactionRemoveAll, value!);
    }
    
    public event Func<GatewayReactionRemoveEmoji, Task>? MessageReactionRemoveEmoji
    {
        add => InsertHandler(GatewayEvent.MessageReactionRemoveEmoji, value!);
        remove => RemoveHandler(GatewayEvent.MessageReactionRemoveEmoji, value!);
    }
    
    public event Func<GatewayMessageAck, Task>? MessageAck
    {
        add => InsertHandler(GatewayEvent.MessageAck, value!);
        remove => RemoveHandler(GatewayEvent.MessageAck, value!);
    }
    
    public event Func<GatewayTypingStart, Task>? TypingStart
    {
        add => InsertHandler(GatewayEvent.TypingStart, value!);
        remove => RemoveHandler(GatewayEvent.TypingStart, value!);
    }
    
    public event Func<GatewayChannelId, Task>? WebhooksUpdate
    {
        add => InsertHandler(GatewayEvent.WebhooksUpdate, value!);
        remove => RemoveHandler(GatewayEvent.WebhooksUpdate, value!);
    }
    
    public event Func<GatewayInviteDelete, Task>? InviteDelete
    {
        add => InsertHandler(GatewayEvent.InviteDelete, value!);
        remove => RemoveHandler(GatewayEvent.InviteDelete, value!);
    }
    
    public event Func<Relationship, Task>? RelationshipAdd
    {
        add => InsertHandler(GatewayEvent.RelationshipAdd, value!);
        remove => RemoveHandler(GatewayEvent.RelationshipAdd, value!);
    }
    
    public event Func<Relationship, Task>? RelationshipUpdate
    {
        add => InsertHandler(GatewayEvent.RelationshipUpdate, value!);
        remove => RemoveHandler(GatewayEvent.RelationshipUpdate, value!);
    }
    
    public event Func<GatewayRelationshipId, Task>? RelationshipRemove
    {
        add => InsertHandler(GatewayEvent.RelationshipRemove, value!);
        remove => RemoveHandler(GatewayEvent.RelationshipRemove, value!);
    }
    
    public event Func<VoiceStateResponse, Task>? VoiceStateUpdate
    {
        add => InsertHandler(GatewayEvent.VoiceStateUpdate, value!);
        remove => RemoveHandler(GatewayEvent.VoiceStateUpdate, value!);
    }
    
    public event Func<GatewayVoiceServer, Task>? VoiceServerUpdate
    {
        add => InsertHandler(GatewayEvent.VoiceServerUpdate, value!);
        remove => RemoveHandler(GatewayEvent.VoiceServerUpdate, value!);
    }
    
    public event Func<GatewayCallSchema, Task>? CallCreate
    {
        add => InsertHandler(GatewayEvent.CallCreate, value!);
        remove => RemoveHandler(GatewayEvent.CallCreate, value!);
    }
    
    public event Func<GatewayCallSchema, Task>? CallUpdate
    {
        add => InsertHandler(GatewayEvent.CallUpdate, value!);
        remove => RemoveHandler(GatewayEvent.CallUpdate, value!);
    }
    
    public event Func<GatewayChannelId, Task>? CallDelete
    {
        add => InsertHandler(GatewayEvent.CallDelete, value!);
        remove => RemoveHandler(GatewayEvent.CallDelete, value!);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private HandlerContainer<T> GetHandlerContainer<T>(string eventType) =>
        (HandlerContainer<T>)_eventHandlers[eventType];
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private HandlerContainer GetHandlerContainer(string eventType) =>
        (HandlerContainer)_eventHandlers[eventType];
    
    private void InsertHandler<T>(string eventType, Func<T, Task> handler)
        => GetHandlerContainer<T>(eventType)
            .InsertDelegate(handler);
    
    private void RemoveHandler<T>(string eventType, Func<T, Task> handler)
        => GetHandlerContainer<T>(eventType)
            .RemoveDelegate(handler);

    private void HandleDispatch(string packetType, object packetData)
    {
        try
        {
            if (_eventHandlers.TryGetValue(packetType, out var handlerContainer))
            {
                Log.EventReceived(_logger, packetType);
                if (packetType == GatewayEvent.Ready)
                {
                    
                }
                
                using (_logger.BeginScope(packetType))
                {
                    handlerContainer.CallHandlersAsync(packetData);
                }
            }
            else
            {
                Log.UnhandledEvent(_logger, packetType);
            }
        }
        catch (DispatchException e)
        {
            Log.DispatchException(_logger, e);
        }
        catch (Exception e)
        {
            Log.UserCodeException(_logger, e);
        }
    }
}