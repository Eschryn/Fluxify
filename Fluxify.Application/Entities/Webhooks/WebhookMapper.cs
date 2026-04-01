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
using Fluxify.Dto.Webhooks;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Webhooks;

[Mapper]
internal partial class WebhookMapper(FluxerApplication fluxerApplication) : IUpdateEntity<Webhook>
{
    [MapperIgnoreSource(nameof(WebhookResponse.ChannelId))]
    [MapperIgnoreSource(nameof(WebhookResponse.GuildId))]
    [MapperIgnoreSource(nameof(WebhookResponse.User))]
    private partial Webhook FromResponse(WebhookResponse dto, GuildTextChannel channel, Guild guild, IUser? createdBy,
        FluxerApplication fluxerApplication);

    [MapperIgnoreSource(nameof(WebhookResponse.ChannelId))]
    [MapperIgnoreSource(nameof(WebhookResponse.GuildId))]
    [MapValue(nameof(Webhook.CreatedBy), null)]
    private partial Webhook FromResponse(WebhookTokenResponse dto, GuildTextChannel channel, Guild guild,
        FluxerApplication fluxerApplication);

    public Webhook FromResponse(WebhookTokenResponse dto)
    {
        var guildTextChannel = fluxerApplication.ChannelsRepository.GetCachedOrDefault<GuildTextChannel>(dto.ChannelId);
        var guild = guildTextChannel switch
        {
            { Guild: var g } => g,
            _ => fluxerApplication.GuildsRepository.Cache.GetCachedOrDefault<Guild>(dto.GuildId)
        };

        return dto switch
        {
            WebhookResponse wr => FromResponse(
                wr,
                guildTextChannel,
                guild,
                ResolveUser(guild, wr),
                fluxerApplication),
            _ => FromResponse(dto, guildTextChannel, guild, fluxerApplication)
        };
    }

    private IUser ResolveUser(Guild? guild, WebhookResponse wr)
    {
        var upsertedUser = fluxerApplication.UsersRepository.Insert(wr.User);
        
        return (IUser?)guild?.MembersRepository.Cache.GetCachedOrDefault<GuildMember>(wr.User.Id) ?? upsertedUser;
    }

    [MapperIgnoreSource(nameof(Webhook.Id))]
    [MapperIgnoreSource(nameof(Webhook.Guild))]
    [MapperIgnoreSource(nameof(Webhook.Channel))]
    public partial void UpdateEntity([MappingTarget] Webhook data, Webhook update);

    public partial WebhookTokenUpdateRequest ToUpdateRequest(WebhookProperties properties);

    [MapperIgnoreSource(nameof(Webhook.Id))]
    [MapperIgnoreSource(nameof(Webhook.Guild))]
    [MapperIgnoreSource(nameof(Webhook.Channel))]
    [MapperIgnoreSource(nameof(Webhook.Token))]
    [MapperIgnoreSource(nameof(Webhook.Avatar))]
    [MapperIgnoreSource(nameof(Webhook.CreatedBy))]
    [MapValue(nameof(WebhookProperties.Avatar), null)]
    public partial WebhookProperties ToProperties(Webhook webhook);
}