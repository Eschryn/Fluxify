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
using Fluxify.Application.State;
using Fluxify.Rest.Webhooks;

namespace Fluxify.Application.Entities.Webhooks;

public class Webhook(FluxerApplication fluxerApplication) : IEntity, ICloneable<Webhook>
{
    private AuthenticatedWebhookRequestBuilder RequestBuilder => field ??= fluxerApplication.Rest.Webhooks[Id, Token];

    public Snowflake Id { get; init; }
    public string Name { get; internal set; }
    public string Token { get; internal set; }
    public MediaHash? Avatar { get; internal set; }
    internal ICacheRef<GuildTextChannel> ChannelRef { get; init; }
    internal CacheRef<Guild> GuildRef { get; init; }
    internal ICacheRef<IUser>? CreatedByRef { get; init; }

    public GuildTextChannel Channel => ChannelRef.Value!;
    public Guild Guild => GuildRef.Value!;
    public IUser? CreatedBy => CreatedByRef?.Value;

    public async Task<Webhook> ModifyAsync(Action<WebhookProperties> properties, CancellationToken cancellationToken = default)
    {
        var updateRequest = fluxerApplication.WebhookMapper.ToUpdateRequest(
            fluxerApplication.WebhookMapper.ToProperties(this)
                .Configure(properties));
        
        var response = await RequestBuilder.UpdateAsync(updateRequest, cancellationToken);
        
        var clonedWebhook = (Webhook)Clone();
        fluxerApplication.WebhookMapper.UpdateEntity(clonedWebhook, response);
        
        return clonedWebhook;
    }

    public Task DeleteAsync(CancellationToken cancellationToken = default) 
        => RequestBuilder.DeleteAsync(cancellationToken);

    public async Task<Message?> SendMessageAsync(
        Action<MessageBuilder> builder,
        string? username = null,
        string? avatarUrl = null,
        [NotNullWhen(true)] bool? wait = null,
        CancellationToken cancellationToken = default
    ) => await RequestBuilder.SendMessageAsync(
        fluxerApplication.MessageMapper.MapToRequest(
            new MessageBuilder()
                .Configure(builder)
                .Build(),
            username,
            avatarUrl
        ),
        wait,
        cancellationToken
    ) is { } response
        ? fluxerApplication.MessageMapper.MapFromResponse(response)
        : null;

    public async Task<Message> EditMessageAsync(
        Message message,
        Action<MessageEdit> edit,
        CancellationToken cancellationToken = default
    ) => await RequestBuilder.UpdateMessageAsync(
        message.Id,
        fluxerApplication.MessageMapper.MapToRequest(
            fluxerApplication.MessageMapper.MapToEdit(message)
                .Configure(edit)
        ),
        cancellationToken
    ) is { } response
        ? fluxerApplication.MessageMapper.MapFromResponse(response)
        : null!;

    public object Clone() => MemberwiseClone();
}