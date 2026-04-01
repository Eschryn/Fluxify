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
            || GuildsRepository.Cache.GetCachedOrDefault<Guild>(arg.GuildId.Value) is not { } guild)
        {
            return;
        }

        var globalUser = UsersRepository.Insert(arg.Member!.User!);
        var user = guild.MembersRepository.Insert(arg.Member, guild, globalUser);

        UpdateGuildUserVoiceState(arg, guild, user);
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
        CurrentUser = (PrivateUser)UsersRepository.Insert(arg.User);
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
        var channel = ChannelsRepository.Cache.GetCachedOrDefault<ITextChannel>(arg.ChannelId);
        var message = channel switch
        {
            GuildTextChannel g => g.MessageRepository.Cache.GetCachedOrDefault<Message>(arg.MessageId),
            PrivateTextChannel p => p.MessageRepository.Cache.GetCachedOrDefault<Message>(arg.MessageId),
            _ => null
        };
        
        var args = new ReactionRemoveEmojiEventArgs(
            mapToEmoji,
            channel!,
            arg.MessageId,
            message,
            channel is GuildTextChannel guildTextChannel ? guildTextChannel.Guild : null
        );
        
        return _messageReactionRemoveEmojiHandlers.CallHandlersAsync(args);
    }

    private Task HandleMessageReactionRemoveAll(GatewayReactionRemoveAll arg)
    {
        var channel = ChannelsRepository.Cache.GetCachedOrDefault<ITextChannel>(arg.ChannelId);
        var message = channel switch
        {
            GuildTextChannel g => g.MessageRepository.Cache.GetCachedOrDefault<Message>(arg.MessageId),
            PrivateTextChannel p => p.MessageRepository.Cache.GetCachedOrDefault<Message>(arg.MessageId),
            _ => null
        };

        
        return _messageReactionRemoveAllHandlers.CallHandlersAsync(new ReactionRemoveAllEventArgs(
            channel is GuildTextChannel guildTextChannel ? guildTextChannel.Guild : null,
            channel!,
            message,
            arg.MessageId
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
        var guild = await GuildsRepository.GetAsync(arg.GuildId);

        foreach (var guildRoleResponse in arg.Roles)
        {
            guild.RolesRepository.Insert(guildRoleResponse, guild);
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
        var guild = await GuildsRepository.GetAsync(arg.GuildId);

        guild.MembersRepository.Delete(arg.User.Id);
    }

    private async Task HandleGuildMemberUpdate(GatewayGuildMember arg)
    {
        var user = UsersRepository.Insert(arg.User!);
        var guild = await GuildsRepository.GetAsync(arg.GuildId);

        guild.MembersRepository.Insert(arg, guild, user);
    }

    private async Task HandleGuildMemberAdd(GatewayGuildMember arg)
    {
        var user = UsersRepository.Insert(arg.User!);
        var guild = await GuildsRepository.GetAsync(arg.GuildId);

        guild.MembersRepository.Insert(arg, guild, user);
    }

    private async Task HandleMessageCreate(GatewayMessage arg)
    {
        var channel = (ITextChannel?)await ChannelsRepository.GetAsync(arg.ChannelId);
        var user = await InsertMessageUser(arg);
        var message = channel switch
        {
            PrivateTextChannel privateTextChannel => privateTextChannel.MessageRepository.InsertNew(arg, user),
            GuildTextChannel guildTextChannel => guildTextChannel.MessageRepository.InsertNew(arg, user),
            _ => await MessageMapper.MapAsync(arg, channel: channel, author: user)
        };

        await _messageCreateHandlers.CallHandlersAsync(new MessageEventArgs(message));
    }

    private async Task HandleMessageUpdate(GatewayMessage arg)
    {
        var channel = (ITextChannel?)await ChannelsRepository.GetAsync(arg.ChannelId);
        var user = await InsertMessageUser(arg);
        var message = channel switch
        {
            PrivateTextChannel privateTextChannel => privateTextChannel.MessageRepository.Update(arg, user),
            GuildTextChannel guildTextChannel => guildTextChannel.MessageRepository.Update(arg, user),
            _ => await MessageMapper.MapAsync(arg, author: user)
        };

        await _messageUpdateHandlers.CallHandlersAsync(new MessageEventArgs(message));
    }

    private async Task HandleMessageDelete(GatewayMessageDelete arg)
    {
        Message? cachedMessage = null;
        var channel = (ITextChannel?)await ChannelsRepository.GetAsync(arg.ChannelId);
        if (channel is GuildTextChannel guildTextChannel)
        {
            guildTextChannel.MessageRepository.Cache.Remove(arg.Id, out cachedMessage);
        }

        await _messageDeleteHandlers.CallHandlersAsync(
            new MessageDeletedEventArgs(
                channel!,
                cachedMessage,
                arg.Id,
                arg.AuthorId,
                arg.Content
            )
        );
    }

    private async Task HandleMessageDeleteBulk(GatewayMessageDeleteBulk arg)
    {
        var channel = (ITextChannel?)await ChannelsRepository.GetAsync(arg.ChannelId);
        var cache = channel switch
        {
            GuildTextChannel { MessageRepository.Cache: OrderedCache<Message, MessageMapper> gtcCache } => gtcCache,
            PrivateTextChannel { MessageRepository.Cache: OrderedCache<Message, MessageMapper> ptcCache } => ptcCache,
            _ => null
        };

        Message[] messages = [];
        cache?.RemoveAll(arg.Ids, out messages);

        await _messageBulkDeletedHandlers.CallHandlersAsync(
            new MessagesBulkDeletedEventArgs(
                channel!,
                arg.Ids,
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
        );
        var user = await guild.MembersRepository.GetAsync(CurrentUser.Id);

        await _guildCreatedHandlers.CallHandlersAsync(new GuildEventArgs(guild, user));
    }

    private async Task HandleGuildUpdate(GuildResponse arg)
    {
        var guild = GuildsRepository.Insert(arg);
        var user = await guild.MembersRepository.GetAsync(CurrentUser.Id);

        await _guildUpdatedHandlers.CallHandlersAsync(new GuildEventArgs(guild, user));
    }

    private async Task HandleGuildDelete(GatewayGuildDelete arg)
    {
        GuildsRepository.Cache.Remove(arg.Id, out var guild);
        GuildMember? user = null;
        if (guild is not null)
        {
            user = await guild.MembersRepository.GetAsync(CurrentUser.Id);
        }

        await _guildDeletedHandlers.CallHandlersAsync(new GuildDeletedEventArgs(guild, user, arg.Id, arg.Unavailable));
    }

    private async Task HandleGuildStickersUpdate(GatewayStickerUpdate arg)
    {
        var guild = await GuildsRepository.GetAsync(arg.GuildId);
        GuildInsertStickers(arg.Stickers, guild);
    }

    private async Task HandleGuildEmojisUpdate(GatewayEmojiUpdate arg)
    {
        var guild = await GuildsRepository.GetAsync(arg.GuildId);
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
        if (ChannelsRepository.GetCachedOrDefault<IChannel>(arg.Id) is IGuildChannel guildChannel)
        {
            guildChannel.Guild.GuildChannels.Remove(guildChannel.Id, out _);
        }

        ChannelsRepository.Remove(arg.Id, out _);

        return Task.CompletedTask;
    }

    private async Task HandleGuildRoleUpdate(GatewayGuildRole arg)
    {
        var guild = await GuildsRepository.GetAsync(arg.GuildId);
        guild.RolesRepository.Insert(arg.Role, guild);
    }

    private async Task HandleGuildRoleDelete(GatewayGuildRoleDelete arg)
    {
        var guild = await GuildsRepository.GetAsync(arg.GuildId);
        guild.RolesRepository.Delete(arg.RoleId);
    }

    private async Task HandleGuildRoleCreate(GatewayGuildRole arg)
    {
        var guild = await GuildsRepository.GetAsync(arg.GuildId);
        guild.RolesRepository.Insert(arg.Role, guild);
    }
}