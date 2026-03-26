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
using Fluxify.Dto.Guilds;
using Fluxify.Dto.Guilds.Emoji;
using Fluxify.Dto.Guilds.Members;
using Fluxify.Dto.Guilds.Roles;
using Fluxify.Dto.Guilds.Stickers;
using Fluxify.Gateway.Model.Data.Channel.Message;

namespace Fluxify.Application;

public partial class FluxerApplication
{
    private Guild InsertGuild(
        GuildResponse response,
        GuildRoleResponse[] roles,
        GuildMemberResponse[] members,
        ChannelResponse[] channels,
        GuildStickerResponse[] stickers,
        GuildEmojiResponse[] emojis
    )
    {
        var guild = Guilds.Insert(response);
        foreach (var guildRoleResponse in roles)
        {
            guild.RolesRepository.Insert(guildRoleResponse, guild);
        }

        foreach (var guildMemberResponse in members)
        {
            guild.MembersRepository.Insert(guildMemberResponse, guild,
                Users.GetCachedOrDefault(guildMemberResponse.User!.Id) ?? Users.Insert(guildMemberResponse.User));
        }

        foreach (var channelResponse in channels)
        {
            InsertChannel(channelResponse, guild);
        }

        GuildInsertStickers(stickers, guild);
        GuildInsertEmoji(emojis, guild);
        
        return guild;
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
        var channel = Channels.Insert(arg);
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
            var guild = await Guilds.GetAsync(arg.GuildId.Value);
            var globalUser = Users.Insert(arg.Author);
            user = guild.MembersRepository.Insert(arg.Member!, guild, globalUser);
        }
        else
        {
            user = Users.Insert(arg.Author);
        }

        return user;
    }
}