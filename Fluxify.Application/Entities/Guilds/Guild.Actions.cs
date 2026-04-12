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
using Fluxify.Application.Entities.Invites;
using Fluxify.Application.Entities.Webhooks;
using Fluxify.Application.Model.Channel;
using Fluxify.Application.Model.Guild;
using Fluxify.Dto.Users.Settings.Security;

namespace Fluxify.Application.Entities.Guilds;

public partial class Guild
{
    public Task<GuildTextChannel> CreateTextChannelAsync(
        string name,
        Action<TextChannelProperties>? configure = null,
        CancellationToken cancellationToken = default
    ) => _app.ChannelsRepository.CreateAsync<GuildTextChannel>(Id, new TextChannelProperties
    {
        Name = name
    }.Configure(configure));

    public Task<GuildVoiceChannel> CreateVoiceChannelAsync(
        string name,
        Action<VoiceChannelProperties>? configure = null
    ) => _app.ChannelsRepository.CreateAsync<GuildVoiceChannel>(Id, new VoiceChannelProperties
    {
        Name = name
    }.Configure(configure));

    public Task<GuildLinkChannel> CreateLinkChannelAsync(
        string name,
        string url,
        Action<LinkChannelProperties>? configure = null
    ) => _app.ChannelsRepository.CreateAsync<GuildLinkChannel>(Id, new LinkChannelProperties
    {
        Name = name,
        Url = url
    }.Configure(configure));

    public Task<GuildCategory> CreateCategoryAsync(
        string name,
        Action<CategoryProperties>? configure = null
    ) => _app.ChannelsRepository.CreateAsync<GuildCategory>(Id, new CategoryProperties
    {
        Name = name
    }.Configure(configure));

    public async Task<Webhook[]> GetWebhooksAsync(
        CancellationToken cancellationToken = default
    ) => (await RequestBuilder.GetWebhooksAsync(cancellationToken))
        ?.Select(_app.WebhookMapper.FromResponse)
        .ToArray() ?? [];

    public async Task<Webhook> GetWebhookAsync(
        Snowflake id,
        CancellationToken cancellationToken = default
    ) => _app.WebhookMapper.FromResponse(
        await _app.Rest.Webhooks[id].GetAsync(cancellationToken) ?? throw new Exception("Webhook was not found"));

    public Task<IGuildMember?> GetMemberAsync(Snowflake id)
        => MembersRepository.GetAsync(id)
            .ContinueWith(t => t.Result.Value, TaskContinuationOptions.OnlyOnRanToCompletion);

    public Task BanAsync(
        Snowflake userId,
        int? deleteMessageDays = null,
        TimeSpan? banDuration = null,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.BanAsync(
        userId,
        deleteMessageDays,
        banDuration,
        reason,
        cancellationToken
    );

    public Task UnbanAsync(
        Snowflake userId,
        string? reason = null,
        CancellationToken cancellationToken = default
    ) => RequestBuilder.UnbanAsync(
        userId,
        reason,
        cancellationToken
    );

    public async Task<IGuildChannel?> GetChannelAsync(
        Snowflake id,
        bool bypassCache = false,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _app.ChannelsRepository.GetAsync(id, bypassCache);

        return result.Value as IGuildChannel;
    }

    public async Task<GuildChannelInviteMetadata[]?> GetInvitesAsync(
        CancellationToken cancellationToken = default
    )
    {
        var invites = await RequestBuilder.ListInvitesAsync(cancellationToken);

        return invites?
            .Select(_app.InviteMapper.MapFromResponse)
            .OfType<GuildChannelInviteMetadata>()
            .ToArray();
    }

    public Task<Guild> UpdateAsync(
        SudoVerificationSchema verificationSchema,
        Action<GuildProperties> update,
        CancellationToken cancellationToken = default
    ) => _app.GuildsRepository.UpdateAsync(this, verificationSchema, update, cancellationToken);
}