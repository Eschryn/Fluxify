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
using System.Globalization;
using System.Text;
using Fluxify.Core.Types;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Webhooks;
using Fluxify.Dto.Webhooks.GitHub;
using Fluxify.Dto.Webhooks.Sentry;
using Fluxify.Dto.Webhooks.Slack;

namespace Fluxify.Rest.Webhooks;

public class AuthenticatedWebhookRequestBuilder(HttpClient httpClient, Snowflake id, string token)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private static readonly CompositeFormat GitHubWebhookUrl = CompositeFormat.Parse("webhooks/{0}/{1}/github");
    private static readonly CompositeFormat SentryWebhookUrl = CompositeFormat.Parse("webhooks/{0}/{1}/sentry");
    private static readonly CompositeFormat SlackWebhookUrl = CompositeFormat.Parse("webhooks/{0}/{1}/slack");
    private static readonly CompositeFormat WebhookUrl = CompositeFormat.Parse("webhooks/{0}/{1}");
    private static readonly CompositeFormat MessagesUrl = CompositeFormat.Parse("webhooks/{0}/{1}/messages/{2}");
    private static readonly CompositeFormat MessagesWaitUrl = CompositeFormat.Parse("webhooks/{0}/{1}/messages/{2}?wait=true");
    private static readonly CompositeFormat WebhookWaitUrl = CompositeFormat.Parse("webhooks/{0}/{1}?wait=true");

    public Task ExecuteAsync(
        GitHubWebhook webhook,
        CancellationToken cancellationToken = default
    ) => httpClient.JsonRequestAsync(
        HttpMethod.Post,
        string.Format(FormatProvider, GitHubWebhookUrl, id, token),
        webhook,
        cancellationToken: cancellationToken
    );

    public Task ExecuteAsync(
        SentryWebhook webhook,
        CancellationToken cancellationToken = default
    ) => httpClient.JsonRequestAsync(
        HttpMethod.Post,
        string.Format(FormatProvider, SentryWebhookUrl, id, token),
        webhook,
        cancellationToken: cancellationToken
    );

    public Task ExecuteAsync(
        SlackWebhookRequest webhook,
        CancellationToken cancellationToken = default
    ) => httpClient.JsonRequestAsync(
        HttpMethod.Post,
        string.Format(FormatProvider, SlackWebhookUrl, id, token),
        webhook,
        cancellationToken: cancellationToken
    );

    public Task<MessageResponse?> SendMessageAsync(
        CreateWebhookMessageRequest request,
        bool? wait = null,
        CancellationToken cancellationToken = default
    ) => wait switch
    {
        true => httpClient.MultipartJsonRequestAsync<CreateWebhookMessageRequest, MessageResponse>(
            HttpMethod.Post,
            string.Format(FormatProvider, WebhookWaitUrl, id, token),
            request,
            cancellationToken: cancellationToken
        ),
        _ => httpClient
            .MultipartJsonRequestAsync(
                HttpMethod.Post,
                string.Format(FormatProvider, WebhookUrl, id, token),
                request,
                cancellationToken: cancellationToken
            ).ContinueWith<MessageResponse?>(
                t => null,
                cancellationToken,
                TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Current
            )
    };

    public Task<MessageResponse?> UpdateMessageAsync(
        Snowflake messageId,
        UpdateMessageRequest request,
        CancellationToken cancellationToken = default
    ) => httpClient.JsonRequestAsync<UpdateMessageRequest, MessageResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, MessagesWaitUrl, id, token, messageId),
        request,
        cancellationToken: cancellationToken
    );

    public Task<WebhookTokenResponse?> GetAsync(CancellationToken cancellationToken = default)
        => httpClient.JsonRequestAsync<WebhookTokenResponse>(
            HttpMethod.Get,
            string.Format(FormatProvider, WebhookUrl, id, token),
            cancellationToken: cancellationToken
        );

    public Task DeleteAsync(CancellationToken cancellationToken = default)
        => httpClient.RequestAsync(
            HttpMethod.Delete,
            string.Format(FormatProvider, WebhookUrl, id, token),
            cancellationToken: cancellationToken
        );

    public Task<WebhookTokenResponse?> UpdateAsync(
        WebhookTokenUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => httpClient.JsonRequestAsync<WebhookTokenUpdateRequest, WebhookTokenResponse>(
        HttpMethod.Patch,
        string.Format(FormatProvider, WebhookUrl, id, token),
        request,
        cancellationToken: cancellationToken
    );
}