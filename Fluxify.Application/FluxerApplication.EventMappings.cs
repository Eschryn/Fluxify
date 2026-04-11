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
using Fluxify.Application.Entities.Guilds.Members;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.EventArgs;
using Fluxify.Application.State;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Guilds;
using Fluxify.Gateway.Model.Data;
using Fluxify.Gateway.Model.Data.Channel;
using Fluxify.Gateway.Model.Data.Channel.Message;
using Fluxify.Gateway.Model.Data.Channel.Reaction;
using Fluxify.Gateway.Model.Data.Guild;
using Fluxify.Gateway.Model.Data.Guild.Roles;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    private void InitializeEvents()
    {
        Gateway.Ready += HandleReady;

        // Message events
        Gateway.MessageCreate += HandleMessageCreate;
        Gateway.MessageUpdate += HandleMessageUpdate;
        Gateway.MessageDelete += HandleMessageDelete;
        Gateway.MessageDeleteBulk += HandleMessageDeleteBulk;

        Gateway.MessageReactionAdd += HandleMessageReactionAdd;
        Gateway.MessageReactionRemove += HandleMessageReactionRemove;
        Gateway.MessageReactionRemoveAll += HandleMessageReactionRemoveAll;
        Gateway.MessageReactionRemoveEmoji += HandleMessageReactionRemoveEmoji;

        Gateway.MessageAck += HandleMessageAck;

        Gateway.SavedMessageCreate += HandleSavedMessageCreate;
        Gateway.SavedMessageDelete += HandleSavedMessageDelete;

        // Channel events
        Gateway.ChannelCreate += HandleChannelCreate;
        Gateway.ChannelUpdate += HandleChannelUpdate;
        Gateway.ChannelUpdateBulk += HandleChannelUpdateBulk;
        Gateway.ChannelDelete += HandleChannelDelete;

        Gateway.ChannelRecipientAdd += HandleChannelRecipientAdd;
        Gateway.ChannelRecipientRemove += HandleChannelRecipientRemove;

        Gateway.ChannelPinsUpdate += HandleChannelPinsUpdate;
        Gateway.ChannelPinsAck += HandleChannelPinsUpdate;

        // Guild events
        Gateway.GuildCreate += HandleGuildCreate;
        Gateway.GuildUpdate += HandleGuildUpdate;
        Gateway.GuildDelete += HandleGuildDelete;

        Gateway.GuildBanAdd += HandleGuildBanAdd;
        Gateway.GuildBanRemove += HandleGuildBanRemove;

        Gateway.GuildEmojisUpdate += HandleGuildEmojisUpdate;
        Gateway.GuildStickersUpdate += HandleGuildStickersUpdate;

        Gateway.GuildMemberAdd += HandleGuildMemberAdd;
        Gateway.GuildMemberUpdate += HandleGuildMemberUpdate;
        Gateway.GuildMemberRemove += HandleGuildMemberRemove;

        Gateway.GuildRoleCreate += HandleGuildRoleCreate;
        Gateway.GuildRoleUpdate += HandleGuildRoleUpdate;
        Gateway.GuildRoleUpdateBulk += HandleGuildRoleUpdate;
        Gateway.GuildRoleDelete += HandleGuildRoleDelete;

        Gateway.PassiveUpdates += HandlePassiveUpdate;

        Gateway.VoiceStateUpdate += HandleVoiceStateUpdate;
    }

    private async Task HandleVoiceStateUpdate(VoiceStateResponse arg)
    {
        if (!arg.GuildId.HasValue
            || GuildsRepository.Cache.GetCachedOrDefault(arg.GuildId.Value) is not { Value: not null } guildRef)
        {
            return;
        }

        var globalUser = UsersRepository.Insert(arg.Member!.User!);
        var user = guildRef.Value.MembersRepository.Insert(arg.Member, guildRef, globalUser);

        UpdateGuildUserVoiceState(arg, guildRef, user);
    }

    private async Task HandlePassiveUpdate(GatewayPassiveUpdate arg)
    {
        var guild = await GuildsRepository.GetAsync(arg.GuildId);
        foreach (var channelChanges in arg.CreatedChannels?.Concat(arg.UpdatedChannels ?? []) ??
                                       arg.UpdatedChannels ?? [])
        {
            ChannelsRepository.Insert(channelChanges);
        }

        foreach (var argDeletedChannel in arg.DeletedChannels ?? [])
        {
            ChannelsRepository.Remove(argDeletedChannel, out _);
        }

        foreach (var voiceState in arg.VoiceStates ?? [])
        {
            await HandleVoiceStateUpdate(voiceState);
        }
    }


    private Task HandleReady(ReadyPayload arg)
    {
        CurrentUserRef = UsersRepository.Insert(arg.User).Cast<PrivateUser>();
        foreach (var channels in arg.PrivateChannels)
        {
            ChannelsRepository.Insert(channels);
        }

        foreach (var userPartialResponse in arg.Users)
        {
            UsersRepository.Insert(userPartialResponse);
        }

        foreach (var guildReadyData in arg.Guilds)
        {
            if (guildReadyData.Unavailable.HasValue || guildReadyData.Properties is null)
            {
                continue;
            }

            InsertGuild(
                guildReadyData.Properties,
                guildReadyData.Roles!,
                guildReadyData.Members!,
                guildReadyData.Channels!,
                guildReadyData.Stickers!,
                guildReadyData.Emojis!,
                guildReadyData.Presences!,
                guildReadyData.VoiceStates!
            );
        }

        return Task.CompletedTask;
    }

    private Task HandleSavedMessageDelete(GatewayMessageIdResponse arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleSavedMessageCreate(MessageResponse arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleMessageAck(GatewayMessageAck arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleMessageReactionRemoveEmoji(GatewayReactionRemoveEmoji arg)
    {
        var mapToEmoji = CommonMapper.MapToEmoji(arg.Emoji);
        var channel = ChannelsRepository.Cache.GetCachedOrDefault(arg.ChannelId);
        var message = channel.Value switch
        {
            GuildTextChannel g => g.MessageRepository.Cache.GetCachedOrDefault(arg.MessageId),
            PrivateTextChannel p => p.MessageRepository.Cache.GetCachedOrDefault(arg.MessageId),
            _ => new CacheRef<Message>(arg.MessageId, null)
        };

        var args = new ReactionRemoveEmojiEventArgs(
            mapToEmoji,
            channel.Cast<ITextChannel>(),
            message,
            channel.Value is GuildTextChannel guildTextChannel ? guildTextChannel.GuildRef : null
        );

        return _messageReactionRemoveEmojiHandlers.CallHandlersAsync(args);
    }

    private Task HandleMessageReactionRemoveAll(GatewayReactionRemoveAll arg)
    {
        var channel = ChannelsRepository.Cache.GetCachedOrDefault(arg.ChannelId);
        var message = channel.Value switch
        {
            GuildTextChannel g => g.MessageRepository.Cache.GetCachedOrDefault(arg.MessageId),
            PrivateTextChannel p => p.MessageRepository.Cache.GetCachedOrDefault(arg.MessageId),
            _ => throw new InvalidOperationException("Channel type not implemented!")
        };

        return _messageReactionRemoveAllHandlers.CallHandlersAsync(new ReactionRemoveAllEventArgs(
            channel.Value is GuildTextChannel guildTextChannel ? guildTextChannel.GuildRef : null,
            channel.Cast<ITextChannel>(),
            message
        ));
    }

    private Task HandleMessageReactionAdd(GatewayReaction arg)
        => _messageReactionAddHandlers.CallHandlersAsync(CreateReactionEventArgs(arg));

    private Task HandleMessageReactionRemove(GatewayReaction arg)
        => _messageReactionRemoveHandlers.CallHandlersAsync(CreateReactionEventArgs(arg));


    private Task HandleChannelPinsUpdate(GatewayChannelPinsAck arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleChannelPinsUpdate(GatewayChannelPinsUpdate arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleChannelRecipientRemove(GatewayGroupChange arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleChannelRecipientAdd(GatewayGroupChange arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleChannelUpdateBulk(GatewayBulkChannelUpdate arg)
    {
        return Task.CompletedTask;
    }

    private async Task HandleGuildRoleUpdate(GatewayGuildRoleBulk arg)
    {
        var guildRef = GuildsRepository.Cache.GetCachedOrDefault(arg.GuildId);
        if (guildRef.Value is null)
        {
            return;
        }

        foreach (var guildRoleResponse in arg.Roles)
        {
            guildRef.Value.RolesRepository.Insert(guildRoleResponse, guildRef);
        }
    }

    private Task HandleGuildBanRemove(GatewayBanData arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleGuildBanAdd(GatewayBanData arg)
    {
        return Task.CompletedTask;
    }

    private async Task HandleGuildMemberRemove(GatewayGuildMemberDelete arg)
    {
        var guild = GuildsRepository.Cache.GetCachedOrDefault(arg.GuildId);

        guild.Value?.MembersRepository.Cache.Remove(arg.User.Id, out var userRef);
    }

    private async Task HandleGuildMemberUpdate(GatewayGuildMember arg)
    {
        var userRef = InsertGuildMemberData(arg);
    }

    private async Task HandleGuildMemberAdd(GatewayGuildMember arg)
    {
        var userRef = InsertGuildMemberData(arg);
    }

    private async Task HandleMessageCreate(GatewayMessage arg)
    {
        await _messageCreateHandlers.CallHandlersAsync(
            new MessageEventArgs((await InsertGatewayMessage(arg)).Value!));
    }

    private async Task HandleMessageUpdate(GatewayMessage arg)
    {
        await _messageUpdateHandlers.CallHandlersAsync(
            new MessageEventArgs((await InsertGatewayMessage(arg)).Value!));
    }

    private async Task HandleMessageDelete(GatewayMessageDelete arg)
    {
        ICacheRef<Message>? cachedMessageRef = null;
        var channel = ChannelsRepository.GetCachedOrDefault(arg.ChannelId);
        if (channel.Value is GuildTextChannel guildTextChannel)
        {
            if (guildTextChannel.MessageRepository.Cache.Remove(arg.Id, out var removedMessageRef))
            {
                cachedMessageRef = removedMessageRef;
            }
        }
        
        await _messageDeleteHandlers.CallHandlersAsync(
            new MessageDeletedEventArgs(
                channel.Cast<ITextChannel>(),
                cachedMessageRef ?? new CacheRef<Message>(arg.Id, null),
                arg.AuthorId.HasValue ? GetMessageUser(arg.AuthorId.Value, channel) : null,
                arg.Content
            )
        );
    }

    private async Task HandleMessageDeleteBulk(GatewayMessageDeleteBulk arg)
    {
        var channel = ChannelsRepository.GetCachedOrDefault(arg.ChannelId);
        var cache = channel.Value switch
        {
            GuildTextChannel { MessageRepository.Cache: OrderedCache<Message, MessageResponse, MessageMapper> gtcCache } => gtcCache,
            PrivateTextChannel { MessageRepository.Cache: OrderedCache<Message, MessageResponse, MessageMapper> ptcCache } => ptcCache,
            _ => null
        };

        CacheRef<Message>[] messages = [];
        cache?.RemoveAll(arg.Ids, out messages);

        await _messageBulkDeletedHandlers.CallHandlersAsync(
            new MessagesBulkDeletedEventArgs(
                channel.Cast<ITextChannel>(),
                messages
            )
        );
    }

    private async Task HandleGuildCreate(GatewayGuildCreate arg)
    {
        var guild = InsertGuild(
            arg.Properties,
            arg.Roles,
            arg.Members,
            arg.Channels,
            arg.Stickers,
            arg.Emojis,
            arg.Presences,
            arg.VoiceStates
        ).Value!;

        await _guildCreatedHandlers.CallHandlersAsync(new GuildEventArgs(guild, (GuildMember)guild.CurrentMember));
    }

    private async Task HandleGuildUpdate(GuildResponse arg)
    {
        var guild = GuildsRepository.Insert(arg).Value!;

        await _guildUpdatedHandlers.CallHandlersAsync(new GuildEventArgs(guild, (GuildMember)guild.CurrentMember));
    }

    private async Task HandleGuildDelete(GatewayGuildDelete arg)
    {
        GuildsRepository.Cache.Remove(arg.Id, out var guild);
        GuildMember? user = null;
        if (guild.Value is not null)
        {
            user = (GuildMember)guild.Value.CurrentMember;
        }

        await _guildDeletedHandlers.CallHandlersAsync(new GuildDeletedEventArgs(guild, user, arg.Unavailable));
    }

    private async Task HandleGuildStickersUpdate(GatewayStickerUpdate arg)
    {
        var guildRef = GuildsRepository.Cache.GetCachedOrDefault(arg.GuildId);
        if (guildRef is not { Value: { } guild })
        {
            return;
        }

        GuildInsertStickers(arg.Stickers, guild);
    }

    private async Task HandleGuildEmojisUpdate(GatewayEmojiUpdate arg)
    {
        var guildRef = GuildsRepository.Cache.GetCachedOrDefault(arg.GuildId);
        if (guildRef is not { Value: { } guild })
        {
            return;
        }

        GuildInsertEmoji(arg.Emojis, guild);
    }

    private async Task HandleChannelCreate(ChannelResponse arg)
    {
        InsertChannel(arg);
    }

    private Task HandleChannelUpdate(ChannelResponse arg)
    {
        InsertChannel(arg);
        return Task.CompletedTask;
    }

    private Task HandleChannelDelete(ChannelResponse arg)
    {
        if (ChannelsRepository.GetCachedOrDefault(arg.Id) is { Value: IGuildChannel guildChannel })
        {
            guildChannel.Guild?.GuildChannels.Remove(guildChannel.Id, out _);
        }

        ChannelsRepository.Remove(arg.Id, out _);

        return Task.CompletedTask;
    }

    private async Task HandleGuildRoleUpdate(GatewayGuildRole arg)
    {
        var guildRef = GuildsRepository.Cache.GetCachedOrDefault(arg.GuildId);
        if (guildRef is not { Value: { } guild })
        {
            return;
        }
        
        guild.RolesRepository.Insert(arg.Role, guildRef);
    }

    private async Task HandleGuildRoleDelete(GatewayGuildRoleDelete arg)
    {
        var guildRef = GuildsRepository.Cache.GetCachedOrDefault(arg.GuildId);
        if (guildRef is not { Value: { } guild })
        {
            return;
        }
        
        guild.RolesRepository.Delete(arg.RoleId);
    }

    private async Task HandleGuildRoleCreate(GatewayGuildRole arg)
    {
        var guildRef = GuildsRepository.Cache.GetCachedOrDefault(arg.GuildId);
        if (guildRef is not { Value: { } guild })
        {
            return;
        }
        
        guild.RolesRepository.Insert(arg.Role, guildRef);
    }
}