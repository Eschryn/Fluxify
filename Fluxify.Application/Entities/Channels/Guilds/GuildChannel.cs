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

using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Extensions;
using Fluxify.Application.Model.Channel;
using Fluxify.Core.Types;
using Fluxify.Rest;
using Fluxify.Rest.Channel;

namespace Fluxify.Application.Entities.Channels.Guilds;

public abstract class GuildChannel<TProperties>(FluxerApplication fluxerApplication) : IGuildChannel
    where TProperties : ChannelProperties
{
    protected FluxerApplication FluxerApplication => fluxerApplication;
    internal ChannelRequestBuilder RequestBuilder => field ??= fluxerApplication.Rest.Channels[Id];
    internal Dictionary<Snowflake, PermissionOverwrite> OverwritesDictionary = new();
    public required Snowflake Id { get; init; }
    public string Name { get; internal set; } = string.Empty;
    public required Guild Guild { get; init; }
    public int? Position { get; internal set; }

    public PermissionOverwrite[]? Overwrites
    {
        get;
        internal set
        {
            field = value;
            OverwritesDictionary = value?.ToDictionary(x => x.Id, x => x) ?? [];
        }
    }
    
    public Task UpdateAsync(
        Action<TProperties> configure,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => fluxerApplication.ChannelsRepository.UpdateAsync(
        this, 
        configure,
        reason,
        cancellationToken
    );

    public Task SetPermissionOverwriteAsync(
        PermissionOverwrite overwrite,
        CancellationToken cancellationToken = default
     ) => RequestBuilder.SetPermissionsOverwriteAsync(
        fluxerApplication.ChannelMapper.FromPermissionOverwrite(overwrite),
        cancellationToken
    );
    
    public Task RemovePermissionOverwriteAsync(
        Snowflake targetId,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.RemovePermissionsOverwriteAsync(targetId, cancellationToken);
    
    //public async Task CreateInviteAsync(CancellationToken cancellationToken = default) 
    //    => await Guild.RequestBuilder.

    protected void AssertPermission(Permissions permissions)
    {
        if (Guild.MembersRepository.Cache.GetCachedOrDefault<GuildMember>(FluxerApplication.CurrentUser.Id) is not {} guildUser)
        {
            return;
        }

        if ((this.CalculateUserPermissions(guildUser) & permissions) != permissions)
        {
            throw new RestApiException(
                "missing_permissions",
                "Bot user does not have sufficient permissions to perform this action.", []);
        }
    }
    
    internal IUser? ResolveUser(Snowflake id) 
        => (IUser?)Guild.MembersRepository.Cache.GetCachedOrDefault<GuildMember>(id) 
           ?? fluxerApplication.UsersRepository.Cache.GetCachedOrDefault<GlobalUser>(id);
    
    public Task DeleteAsync() => FluxerApplication.ChannelsRepository.DeleteAsync(Id);

    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        "i" or "I" => ((long)Id).ToString(),
        _ or "m" or "M" => $"<#{Id}>"
    };
}