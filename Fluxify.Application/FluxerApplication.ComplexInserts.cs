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
using UserStatus = Fluxify.Gateway.Model.Data.UserStatus;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    private Guild InsertGuild(
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
        var guild = GuildsRepository.Insert(response);
        foreach (var guildRoleResponse in roles)
        {
            guild.RolesRepository.Insert(guildRoleResponse, guild);
        }

        foreach (var guildMemberResponse in members)
        {
            guild.MembersRepository.Insert(guildMemberResponse, guild,
                UsersRepository.GetCachedOrDefault(guildMemberResponse.User!.Id) ?? UsersRepository.Insert(guildMemberResponse.User));
        }

        foreach (var channelResponse in channels)
        {
            InsertChannel(channelResponse, guild);
        }

        foreach (var voiceStateResponse in voiceStates)
        {
            if (guild.MembersRepository.Cache.GetCachedOrDefault<GuildMember>(voiceStateResponse.UserId) is not { } user)
            {
                continue;
            }

            UpdateGuildUserVoiceState(voiceStateResponse, guild, user);
        }

        foreach (var presence in presences)
        {
            UpdateUserPresence(presence);
        }
        
        GuildInsertStickers(stickers, guild);
        GuildInsertEmoji(emojis, guild);
        
        return guild;
    }

    private void UpdateUserPresence(PresenceResponse presence)
    {
        if (UsersRepository.GetCachedOrDefault(presence.User.Id) is {} user)
        {
            _userMapper.UpdateStatus(user, presence);
        }
    }

    private void UpdateGuildUserVoiceState(VoiceStateResponse voiceState, Guild guild, GuildMember guildMember)
    {
        if (voiceState.ConnectionId == null)
        {
            return;
        }

        if (voiceState.ChannelId == null)
        {
            guildMember.VoiceStateList.TryRemove(voiceState.ConnectionId, out _);
            return;
        }
        
        guildMember.VoiceStateList.AddOrUpdate(
            voiceState.ConnectionId,
            _ =>
            {
                var state = new VoiceState
                {
                    VoiceChannel = (GuildVoiceChannel)guild.Channels[voiceState.ChannelId.Value]
                };
                _userMapper.UpdateVoiceState(state, voiceState);
                
                return state;
            },
            (_, target) =>
            {
                _userMapper.UpdateVoiceState(target, voiceState);
                return target;
            }
        );
    }
    
    private static void GuildInsertStickers(GuildStickerResponse[] stickers, Guild guild)
    {
        foreach (var guildStickerResponse in stickers)
        {
            guild.GuildStickers.AddOrUpdate(
                guildStickerResponse.Id,
                _ => CommonMapper.MapToSticker(guildStickerResponse),
                (_, target) =>
                {
                    CommonMapper.UpdateSticker(target, guildStickerResponse);
                    return target;
                });
        }
    }

    private static void GuildInsertEmoji(GuildEmojiResponse[] emojis, Guild guild)
    {
        foreach (var guildEmojiResponse in emojis)
        {
            guild.GuildEmojis.AddOrUpdate(
                guildEmojiResponse.Id!.Value,
                _ => (GuildEmoji)CommonMapper.MapToEmoji(guildEmojiResponse),
                (_, target) =>
                {
                    target.Name = guildEmojiResponse.Name;
                    return target;
                });
        }
    }

    private void InsertChannel(ChannelResponse arg, Guild? guild = null)
    {
        var channel = ChannelsRepository.Insert(arg);
        if (channel is IGuildChannel guildChannel)
        {
            guild ??= guildChannel.Guild;
        
            guild?.GuildChannels.AddOrUpdate(
                channel.Id,
                _ => guildChannel,
                (_, target) =>
                {
                    _channelMapper.UpdateEntity(target, guildChannel);
                    return target;
                });
        }
    }

    private async Task<IUser> InsertMessageUser(GatewayMessage arg)
    {
        IUser user;
        if (arg.WebhookId.HasValue)
        {
            user = _userMapper.MapWebhook(arg.Author);
        }
        else if (arg.GuildId.HasValue)
        {
            var guild = await GuildsRepository.GetAsync(arg.GuildId.Value);
            var globalUser = UsersRepository.Insert(arg.Author);
            user = guild.MembersRepository.Insert(arg.Member!, guild, globalUser);
        }
        else
        {
            user = UsersRepository.Insert(arg.Author);
        }

        return user;
    }

    private ReactionEventArgs CreateReactionEventArgs(GatewayReaction arg)
    {
        IUser? user;
        Guild? guild = null;
        Message? message = null;
        var emoji = CommonMapper.MapToEmoji(arg.Emoji);

        if (arg.GuildId.HasValue)
        {
            guild = GuildsRepository.Cache.GetCachedOrDefault<Guild>(arg.GuildId.Value);
        }

        if (arg.Member is not null)
        {
            user = UsersRepository.Insert(arg.Member.User!);
            if (guild is not null)
            {
                user = guild.MembersRepository.Insert(arg.Member, guild, (GlobalUser)user);
            }
        }
        else
        {
            // hope
            user = UsersRepository.GetCachedOrDefault(arg.UserId);
        }

        var channel = ChannelsRepository.Cache.GetCachedOrDefault<ITextChannel>(arg.ChannelId);
        if (channel is not null)
        {
            var repo = channel switch
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
                }, out message);
            }
        }

        return new ReactionEventArgs(
            emoji,
            channel!,
            arg.UserId,
            arg.GuildId,
            arg.MessageId,
            message,
            guild,
            user,
            arg.SessionId
        );
    }
}