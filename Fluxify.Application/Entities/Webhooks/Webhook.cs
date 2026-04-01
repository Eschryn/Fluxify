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

using System.Diagnostics.CodeAnalysis;
using Fluxify.Application.Entities.Channels.Guilds;
using Fluxify.Application.Entities.Guilds;
using Fluxify.Application.Entities.Messages;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Model.Channel;
using Fluxify.Application.Model.Messages;
using Fluxify.Core.Types;
using Fluxify.Dto.Common;
using Fluxify.Rest.Webhooks;

namespace Fluxify.Application.Entities.Webhooks;

public class Webhook(FluxerApplication fluxerApplication) : IEntity
{
    private AuthenticatedWebhookRequestBuilder RequestBuilder => field ??= fluxerApplication.Rest.Webhooks[Id, Token];

    public Snowflake Id { get; init; }
    public string Name { get; internal set; }
    public string Token { get; internal set; }
    public MediaHash? Avatar { get; internal set; }
    public required GuildTextChannel Channel { get; init; }
    public required Guild Guild { get; init; }
    public IUser? CreatedBy { get; internal set; }

    public async Task ModifyAsync(Action<WebhookProperties> properties, CancellationToken cancellationToken = default)
        => fluxerApplication.WebhookMapper.UpdateEntity(
            this,
            fluxerApplication.WebhookMapper.FromResponse(
                await RequestBuilder.UpdateAsync(
                    fluxerApplication.WebhookMapper.ToUpdateRequest(
                        fluxerApplication.WebhookMapper.ToProperties(this)
                            .Configure(properties)))));
    
    public Task DeleteAsync(CancellationToken cancellationToken = default) 
        => RequestBuilder.DeleteAsync(cancellationToken);

    public async Task<Message?> SendMessageAsync(
        Action<MessageBuilder> builder,
        string? username = null,
        string? avatarUrl = null,
        [NotNullWhen(true)] bool? wait = null,
        CancellationToken cancellationToken = default
    ) => await RequestBuilder.SendMessageAsync(
        fluxerApplication.MessageMapper.Map(
            new MessageBuilder()
                .Configure(builder)
                .Build(),
            username,
            avatarUrl
        ),
        wait,
        cancellationToken
    ) is { } response
        ? await fluxerApplication.MessageMapper.MapAsync(response)
        : null;

    public async Task<Message> EditMessageAsync(
        Message message,
        Action<MessageEdit> edit,
        CancellationToken cancellationToken = default
    ) => await RequestBuilder.UpdateMessageAsync(
        message.Id,
        fluxerApplication.MessageMapper.Map(
            fluxerApplication.MessageMapper.MapToEdit(message)
                .Configure(edit)
        ),
        cancellationToken
    ) is { } response
        ? await fluxerApplication.MessageMapper.MapAsync(response)
        : null!;
}