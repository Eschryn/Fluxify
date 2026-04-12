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
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Extensions;
using Fluxify.Application.Model.Channel;
using Fluxify.Rest;
using Fluxify.Rest.Channel;

namespace Fluxify.Application.Entities.Channels.Guilds;

public abstract class GuildChannel<TSelf, TProperties> : IGuildChannel
    where TSelf : GuildChannel<TSelf, TProperties>
    where TProperties : ChannelProperties
{
    protected FluxerApplication FluxerApplication => _fluxerApplication;
    internal ChannelRequestBuilder RequestBuilder => field ??= _fluxerApplication.Rest.Channels[Id];
    internal ImmutableDictionary<Snowflake, PermissionOverwrite> OverwritesDictionary { get; private set; } 
        = ImmutableDictionary<Snowflake, PermissionOverwrite>.Empty;
    internal readonly CacheRef<Guild> GuildRef;
    private readonly FluxerApplication _fluxerApplication;

    protected GuildChannel(
        FluxerApplication fluxerApplication,
        CacheRef<Guild> guildRef
    )
    {
        _fluxerApplication = fluxerApplication;
        GuildRef = guildRef;
        Guild = guildRef.Value!;
    }

    public required Snowflake Id { get; init; }
    public string Name { get; internal set; } = string.Empty;
    public Guild Guild => field = GuildRef.Value ?? field;
    public int? Position { get; internal set; }
    public string DisplayName => Name;

    public PermissionOverwrite[]? PermissionOverwrites
    {
        get;
        internal set
        {
            field = value;
            OverwritesDictionary = value?.ToImmutableDictionary(x => x.Id, x => x) 
                                   ?? ImmutableDictionary<Snowflake, PermissionOverwrite>.Empty;
        }
    }
    
    public Task<TSelf> UpdateAsync(
        Action<TProperties> configure,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => _fluxerApplication.ChannelsRepository.UpdateAsync(
        (TSelf)this, 
        configure,
        reason,
        cancellationToken
    );

    public Task SetPermissionOverwriteAsync(
        PermissionOverwrite overwrite,
        CancellationToken cancellationToken = default
     ) => RequestBuilder.SetPermissionsOverwriteAsync(
        _fluxerApplication.ChannelMapper.ToDto(overwrite),
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
        if (Guild?.MembersRepository.Cache.GetCachedOrDefault(FluxerApplication.CurrentUser.Id).Value is not {} guildUser)
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
        => (IUser?)Guild?.MembersRepository.Cache.GetCachedOrDefault(id).Value 
           ?? _fluxerApplication.UsersRepository.Cache.GetCachedOrDefault(id).Value;
    
    public Task DeleteAsync() => FluxerApplication.ChannelsRepository.DeleteAsync(Id);

    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        "i" or "I" => ((long)Id).ToString(),
        _ => $"<#{Id}>"
    };

    public object Clone() => MemberwiseClone();
}