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
    }

    private async Task HandlePassiveUpdate(GatewayPassiveUpdate arg)
    {
        var guild = await Guilds.GetAsync(arg.GuildId);
        foreach (var channelChanges in arg.CreatedChannels?.Concat(arg.UpdatedChannels ?? []) ?? arg.UpdatedChannels ?? [])
        {
            Channels.Insert(channelChanges);
        }

        foreach (var argDeletedChannel in arg.DeletedChannels ?? [])
        {
            Channels.Cache.Remove(argDeletedChannel);
        }
        
        // TODO: Handle voice state updates
        // arg.VoiceStates[0].
    }


    private Task HandleReady(ReadyPayload arg)
    {
        CurrentUser = (PrivateUser)Users.Insert(arg.User);
        foreach (var channels in arg.PrivateChannels)
        {
            Channels.Insert(channels);
        }
        
        foreach (var userPartialResponse in arg.Users)
        {
            Users.Insert(userPartialResponse);
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
                guildReadyData.Emojis!
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
        return Task.CompletedTask;
    }

    private Task HandleMessageReactionRemoveAll(GatewayReactionRemoveAll arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleMessageReactionRemove(GatewayReaction arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleMessageReactionAdd(GatewayReaction arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleMessageDeleteBulk(GatewayMessageDeleteBulk arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleMessageDelete(GatewayMessageDelete arg)
    {
        return Task.CompletedTask;
    }

    private Task HandleMessageUpdate(MessageResponse arg)
    {
        return Task.CompletedTask;
    }

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
        var guild = await Guilds.GetAsync(arg.GuildId);

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
        var guild = await Guilds.GetAsync(arg.GuildId);

        guild.MembersRepository.Delete(arg.User.Id);
    }

    private async Task HandleGuildMemberUpdate(GatewayGuildMember arg)
    {
        var user = Users.Insert(arg.User!);
        var guild = await Guilds.GetAsync(arg.GuildId);

        guild.MembersRepository.Insert(arg, guild, user);
    }

    private async Task HandleGuildMemberAdd(GatewayGuildMember arg)
    {
        var user = Users.Insert(arg.User!);
        var guild = await Guilds.GetAsync(arg.GuildId);

        guild.MembersRepository.Insert(arg, guild, user);
    }

    private async Task HandleMessageCreate(GatewayMessageCreate arg)
    {
        IUser user;
        if (arg.WebhookId.HasValue)
        {
            user = _userMapper.MapWebhook(arg.Author);
        }
        else if (arg.GuildId.HasValue)
        {
            var guild = await Guilds.GetAsync(arg.GuildId.Value);
            var globalUser = Users.Insert(arg.Author);
            user = guild.MembersRepository.Insert(arg.Member!, guild, globalUser);
        }
        else
        {
            user = Users.Insert(arg.Author);
        }

        var message = await MessageMapper.MapAsync(arg, user);
        await _messageHandlers.CallHandlersAsync(message);
    }

    private async Task HandleGuildCreate(GatewayGuildCreate arg)
    {
        var guild = InsertGuild(
            arg.Properties,
            arg.Roles,
            arg.Members,
            arg.Channels,
            arg.Stickers,
            arg.Emojis
        );
        
        await _guildCreatedHandlers.CallHandlersAsync(guild);
    }

    private async Task HandleGuildUpdate(GuildResponse arg)
        => await _guildUpdatedHandlers.CallHandlersAsync(Guilds.Insert(arg));

    private async Task HandleGuildDelete(GatewayGuildDelete arg)
    {
        try
        {
            var guild = Guilds.Cache.GetCachedOrDefault<Guild>(arg.Id);
            await _guildDeletedHandlers.CallHandlersAsync(guild!);
        }
        finally
        {
            Guilds.Cache.Remove(arg.Id);
        }
    }

    private async Task HandleGuildStickersUpdate(GatewayStickerUpdate arg)
    {
        var guild = await Guilds.GetAsync(arg.GuildId);
        GuildInsertStickers(arg.Stickers, guild);
    }

    private async Task HandleGuildEmojisUpdate(GatewayEmojiUpdate arg)
    {
        var guild = await Guilds.GetAsync(arg.GuildId);
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
        if (Channels.GetCachedOrDefault<IChannel>(arg.Id) is IGuildChannel guildChannel)
        {
            guildChannel.Guild.GuildChannels.Remove(guildChannel.Id, out _);
        }

        Channels.Cache.Remove(arg.Id);

        return Task.CompletedTask;
    }

    private async Task HandleGuildRoleUpdate(GatewayGuildRole arg)
    {
        var guild = await Guilds.GetAsync(arg.GuildId);
        guild.RolesRepository.Insert(arg.Role, guild);
    }

    private async Task HandleGuildRoleDelete(GatewayGuildRoleDelete arg)
    {
        var guild = await Guilds.GetAsync(arg.GuildId);
        guild.RolesRepository.Delete(arg.RoleId);
    }

    private async Task HandleGuildRoleCreate(GatewayGuildRole arg)
    {
        var guild = await Guilds.GetAsync(arg.GuildId);
        guild.RolesRepository.Insert(arg.Role, guild);
    }
}