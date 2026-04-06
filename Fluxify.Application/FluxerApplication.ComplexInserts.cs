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

using System.Collections.Immutable;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.EventArgs;
using Fluxify.Application.State.Ref;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels;
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.Emoji;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Roles;
using Fluxify.Dto.Guilds.Stickers;
using Fluxify.Gateway.Model.Data.Channel.Message;
using Fluxify.Gateway.Model.Data.Channel.Reaction;
using Fluxify.Gateway.Model.Data.User;
using Fluxify.Gateway.Model.Data.Voice;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    private CacheRef<Guild> InsertGuild(
        GuildResponse response,
        GuildRoleResponse[] roles,
        GuildMemberResponse[] members,
        ChannelResponse[] channels,
        GuildStickerResponse[] stickers,
        GuildEmojiResponse[] emojis,
        PresenceResponse[] presences,
        VoiceStateResponse[] voiceStates
    )
    {
        var guildRef = GuildsRepository.Insert(response);
        var guild = guildRef.Value!;
        foreach (var guildRoleResponse in roles)
        {
            guild.RolesRepository.Insert(guildRoleResponse, guildRef);
        }

        foreach (var guildMemberResponse in members)
        {
            guild.MembersRepository.Insert(
                guildMemberResponse,
                guildRef,
                UsersRepository.Insert(guildMemberResponse.User!));
        }

        foreach (var channelResponse in channels)
        {
            InsertChannel(channelResponse, guildRef);
        }

        foreach (var voiceStateResponse in voiceStates)
        {
            if (guild.MembersRepository.Cache.GetCachedOrDefault(voiceStateResponse.UserId) is not { Value: not null } user)
            {
                continue;
            }

            UpdateGuildUserVoiceState(voiceStateResponse, guildRef, user);
        }

        foreach (var presence in presences)
        {
            UpdateUserPresence(presence);
        }
        
        GuildInsertStickers(stickers, guild);
        GuildInsertEmoji(emojis, guild);
        
        return guildRef;
    }

    private void UpdateUserPresence(PresenceResponse presence)
    {
        UsersRepository.Cache.TryUpdate(
            presence.User.Id,
            user => UserMapper.UpdateStatus(user, presence),
            out _);
    }

    private void UpdateGuildUserVoiceState(
        VoiceStateResponse voiceState,
        CacheRef<Guild> guildRef,
        CacheRef<IGuildMember> guildMemberRef
    )
    {
        if (voiceState.ConnectionId == null 
            || guildMemberRef.Value is not GuildMember { VoiceStateList: var voiceStateList}
            || guildRef.Value is not {} guild)
        {
            return;
        }

        if (voiceState.ChannelId == null)
        {
            voiceStateList.TryRemove(voiceState.ConnectionId, out _);
            return;
        }
        
        voiceStateList.AddOrUpdate(
            voiceState.ConnectionId,
            _ =>
            {
                var state = new VoiceState
                {
                    VoiceChannelRef = guild.GuildChannels.GetCachedOrDefault(voiceState.ChannelId.Value).Cast<GuildVoiceChannel>()
                };
                UserMapper.UpdateVoiceState(state, voiceState);
                
                return state;
            },
            (_, target) =>
            {
                UserMapper.UpdateVoiceState(target, voiceState);
                return target;
            }
        );
    }
    
    private static void GuildInsertStickers(GuildStickerResponse[] stickers, Guild guild)
    {
        guild.GuildStickers = stickers
            .Select(CommonMapper.MapToSticker)
            .ToImmutableDictionary(k => k.Id);
    }

    private static void GuildInsertEmoji(GuildEmojiResponse[] emojis, Guild guild)
    {
        guild.GuildEmojis = emojis
            .Select(CommonMapper.MapToEmoji)
            .OfType<GuildEmoji>()
            .ToImmutableDictionary(k => k.Id);
    }

    private CacheRef<IChannel> InsertChannel(ChannelResponse arg, CacheRef<Guild>? guildRef = null)
    {
        var channel = ChannelsRepository.Insert(arg, guildRef);
        if (channel.Value is IGuildChannel guildChannel)
        {
            var guild = guildRef?.Value ?? guildChannel.Guild; 
            guild?.GuildChannels.UpdateOrCreate(guildChannel);
        }
        
        return channel;
    }

    private async Task<ICacheRef<IUser>> InsertMessageUser(GatewayMessage arg)
    {
        ICacheRef<IUser> user;
        if (arg.WebhookId.HasValue)
        {
            user = new CacheRef<WebhookUser>(arg.WebhookId.Value, UserMapper.MapWebhook(arg.Author));
        }
        else if (arg.GuildId.HasValue)
        {
            var guild = await GuildsRepository.GetAsync(arg.GuildId.Value);
            var globalUser = UsersRepository.Insert(arg.Author);
            user = guild.Value!.MembersRepository.Insert(arg.Member!, guild, globalUser);
        }
        else
        {
            user = UsersRepository.Insert(arg.Author);
        }

        return user;
    }

    private ReactionEventArgs CreateReactionEventArgs(GatewayReaction arg)
    {
        ICacheRef<IUser>? userRef;
        CacheRef<Guild>? guildRef = null;
        CacheRef<Message>? messageRef = null;
        var emoji = CommonMapper.MapToEmoji(arg.Emoji);

        if (arg.GuildId.HasValue)
        {
            guildRef = GuildsRepository.Cache.GetCachedOrDefault(arg.GuildId.Value);
        }

        if (arg.Member is not null)
        {
            userRef = UsersRepository.Insert(arg.Member.User!);
            if (guildRef?.Value != null)
            {
                userRef = guildRef.Value.MembersRepository.Insert(
                    arg.Member,
                    guildRef,
                    // we assigned CacheRef<GlobalUser> above so this cast is safe
                    (CacheRef<GlobalUser>)userRef.Cast<GlobalUser>());
            }
        }
        else
        {
            // hope
            userRef = UsersRepository.GetCachedOrDefault(arg.UserId);
        }

        var channelRef = ChannelsRepository.Cache.GetCachedOrDefault(arg.ChannelId);
        if (channelRef.Value is not null)
        {
            var repo = channelRef.Value switch
            {
                GuildTextChannel guildTextChannel => guildTextChannel.MessageRepository,
                PrivateTextChannel privateTextChannel => privateTextChannel.MessageRepository,
                _ => null
            };

            if (repo is not null)
            {
                repo.Cache.TryUpdate(arg.MessageId, msg =>
                {
                    var reaction = msg.Reactions?.FirstOrDefault(r => r.Emoji == emoji);
                    reaction?.Count += 1;
                    reaction?.Me = reaction.Me || arg.UserId == CurrentUser.Id;
                }, out var messageRefResult);
                
                messageRef = messageRefResult;
            }
        }

        return new ReactionEventArgs(
            emoji,
            channelRef.Cast<ITextChannel>(),
            messageRef,
            guildRef,
            userRef
        );
    }

    private ICacheRef<IUser> InsertGuildMemberData(GatewayGuildMember arg)
    {
        var userRef = UsersRepository.Insert(arg.User!);
        var guildRef = GuildsRepository.Cache.GetCachedOrDefault(arg.GuildId);
        if (guildRef.Value is not {} guild)
        {
            return userRef;
        }

        return guild.MembersRepository.Insert(arg, guildRef, userRef);
    }

    private async Task<CacheRef<Message>> InsertGatewayMessage(GatewayMessage arg)
    {
        var channel = ChannelsRepository.Cache.GetCachedOrDefault(arg.ChannelId);
        var user = await InsertMessageUser(arg);
        var message = channel.Value switch
        {
            PrivateTextChannel privateTextChannel => privateTextChannel.MessageRepository.InsertNew(arg, user),
            GuildTextChannel guildTextChannel => guildTextChannel.MessageRepository.InsertNew(arg, user),
            _ => new CacheRef<Message>(arg.Id, MessageMapper.Map(arg, channel: channel, author: user))
        };
        return message;
    }

    private ICacheRef<IUser> GetMessageUser(Snowflake userId, CacheRef<IChannel> channelRef)
    {
        if (channelRef.Value is IGuildChannel { Guild.MembersRepository: { } membersRepository })
        {
            return membersRepository.Cache.GetCachedOrDefault(userId);
        }

        return UsersRepository.Cache.GetCachedOrDefault(userId);
    }
}