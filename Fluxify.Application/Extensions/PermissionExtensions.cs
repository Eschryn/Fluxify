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

using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Channel;
using Fluxify.Core.Types;

namespace Fluxify.Application.Extensions;

public static class PermissionExtensions
{
    extension(IGuildChannel guildChannel)
    {
        public Permissions CalculateUserPermissions(IGuildMember member) 
            => guildChannel switch
            {
                GuildVoiceChannel voiceChannel => voiceChannel.CalculateUserPermissions(member),
                GuildTextChannel textChannel => textChannel.CalculateUserPermissions(member),
                GuildLinkChannel linkChannel => linkChannel.CalculateUserPermissions(member),
                GuildCategory category => category.CalculateUserPermissions(member),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
    
    extension<TSelf, TProperties>(GuildChannel<TSelf, TProperties> guildChannel)
        where TSelf : GuildChannel<TSelf, TProperties>
        where TProperties : ChannelProperties
    {
        internal Permissions CalculateUserPermissions(IGuildMember member)
        {
            var guildChannelGuild = guildChannel.Guild;
            if (guildChannelGuild == null)
            {
                throw new InvalidOperationException("Guild is null");
            }

            if (guildChannelGuild.Owner.Id == member.Id)
            {
                return (Permissions)ulong.MaxValue;
            }
            
            var everyone = guildChannelGuild.RolesRepository.Cache.GetCachedOrDefault(guildChannelGuild.Id).Value!;
            var permissionResult = everyone.Permissions;

            Permissions allowed, denied;
            if (guildChannel.OverwritesDictionary.TryGetValue(everyone.Id, out var overwrite))
            {
                allowed = overwrite.Allow ?? Permissions.None;
                denied = overwrite.Deny ?? Permissions.None;
                permissionResult = permissionResult & ~denied | allowed;
            }
        
            denied = Permissions.None;
            allowed = Permissions.None;
            foreach (var role in member.Roles)
            {
                permissionResult |= role.Permissions;

                if (guildChannel.OverwritesDictionary.TryGetValue(role.Id, out overwrite))
                {
                    allowed |= overwrite.Allow ?? Permissions.None;
                    denied |= overwrite.Deny ?? Permissions.None;
                }
            }
        
            permissionResult = permissionResult & ~denied | allowed;

            if (guildChannel.OverwritesDictionary.TryGetValue(member.Id, out overwrite))
            {
                allowed = overwrite.Allow ?? Permissions.None;
                denied = overwrite.Deny ?? Permissions.None;
                return permissionResult & ~denied | allowed;
            }

            return permissionResult;
        }
    } 
}