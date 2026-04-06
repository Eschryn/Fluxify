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
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.State.Ref;
using Fluxify.Dto.Webhooks;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Webhooks;

[Mapper]
internal partial class WebhookMapper(FluxerApplication fluxerApplication) : IUpdateEntity<Webhook>
{

    [MapperIgnoreSource(nameof(WebhookResponse.ChannelId))]
    [MapperIgnoreSource(nameof(WebhookResponse.GuildId))]
    [MapperIgnoreSource(nameof(WebhookResponse.User))]
    [MapperIgnoreTarget(nameof(Webhook.Guild))]
    private partial Webhook FromResponse(WebhookResponse dto, ICacheRef<GuildTextChannel> channelRef, CacheRef<Guild> guildRef, ICacheRef<IUser>? createdByRef,
        FluxerApplication fluxerApplication);

    [MapperIgnoreSource(nameof(WebhookResponse.ChannelId))]
    [MapperIgnoreSource(nameof(WebhookResponse.GuildId))]
    [MapValue(nameof(Webhook.CreatedByRef), null)]
    private partial Webhook FromResponse(WebhookTokenResponse dto, ICacheRef<GuildTextChannel> channelRef, CacheRef<Guild> guildRef,
        FluxerApplication fluxerApplication);
    
    public Webhook FromResponse(WebhookTokenResponse dto)
    {
        var guildTextChannel = fluxerApplication.ChannelsRepository
            .GetCachedOrDefault(dto.ChannelId)
            .Cast<GuildTextChannel>();

        var guild = guildTextChannel.Value switch
        {
            { GuildRef: var g } => g,
            _ => fluxerApplication.GuildsRepository.Cache.GetCachedOrDefault(dto.GuildId)
        };

        return dto switch
        {
            WebhookResponse wr => FromResponse(
                wr,
                guildTextChannel,
                guild,
                ResolveUser(guild.Value, wr),
                fluxerApplication),
            _ => FromResponse(dto, guildTextChannel, guild, fluxerApplication)
        };
    }

    private ICacheRef<IUser> ResolveUser(Guild? guild, WebhookResponse wr)
    {
        var upsertedUserRef = fluxerApplication.UsersRepository.Insert(wr.User);

        return guild?.MembersRepository.Cache.GetCachedOrDefault(wr.User.Id) is { Value: not null } memberRef
            ? memberRef
            : upsertedUserRef;
    }

    [MapperIgnoreSource(nameof(Webhook.Id))]
    [MapperIgnoreSource(nameof(Webhook.Guild))]
    [MapperIgnoreSource(nameof(Webhook.Channel))]
    [MapperIgnoreSource(nameof(Webhook.ChannelRef))]
    [MapperIgnoreSource(nameof(Webhook.GuildRef))]
    public partial void UpdateEntity([MappingTarget] Webhook data, Webhook update);

    public partial WebhookTokenUpdateRequest ToUpdateRequest(WebhookProperties properties);

    [MapperIgnoreSource(nameof(Webhook.Id))]
    [MapperIgnoreSource(nameof(Webhook.Guild))]
    [MapperIgnoreSource(nameof(Webhook.Channel))]
    [MapperIgnoreSource(nameof(Webhook.Token))]
    [MapperIgnoreSource(nameof(Webhook.Avatar))]
    [MapperIgnoreSource(nameof(Webhook.CreatedBy))]
    [MapperIgnoreSource(nameof(Webhook.ChannelRef))]
    [MapperIgnoreSource(nameof(Webhook.GuildRef))]
    [MapperIgnoreSource(nameof(Webhook.CreatedByRef))]
    [MapValue(nameof(WebhookProperties.Avatar), null)]
    public partial WebhookProperties ToProperties(Webhook webhook);
}