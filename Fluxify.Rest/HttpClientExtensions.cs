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

using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Fluxify.Dto.Uploads;
using Fluxify.Rest.Model;

namespace Fluxify.Rest;

internal static class HttpClientExtensions
{
    private static readonly CompositeFormat FileFormat = CompositeFormat.Parse("files[{0}]");

    extension(HttpClient client)
    {
        public async Task<TResult> MultipartJsonRequestAsync<TRequest, TResult>(
            HttpMethod method,
            string url,
            TRequest request,
            JsonTypeInfo<TRequest> requestJsonInfo,
            JsonTypeInfo<TResult> resultJsonInfo,
            CancellationToken cancellationToken = default)
            where TRequest : MultipartDto
            where TResult : class
        {
            if (request.Files is not { Length: > 0 })
            {
                return await client.JsonRequestAsync(
                    method,
                    url,
                    request,
                    requestJsonInfo,
                    resultJsonInfo,
                    reason: null,
                    cancellationToken: cancellationToken
                );
            }

            using var httpResponseMessage =
                await client.MultipartJsonRequestImpl(method, url, request, requestJsonInfo, cancellationToken);

            return await HttpClient.DeserializeResponseAsync(resultJsonInfo, cancellationToken, httpResponseMessage);
        }

        public async Task MultipartJsonRequestAsync<TRequest>(
            HttpMethod method,
            string url,
            TRequest request,
            JsonTypeInfo<TRequest> requestJsonInfo,
            CancellationToken cancellationToken = default)
            where TRequest : MultipartDto
        {
            if (request.Files is not { Length: > 0 })
            {
                await client.JsonRequestAsync(method, url, request, requestJsonInfo, reason: null, cancellationToken: cancellationToken);

                return;
            }

            using var httpResponseMessage =
                await client.MultipartJsonRequestImpl(method, url, request, requestJsonInfo, cancellationToken);
        }

        private async Task<HttpResponseMessage> MultipartJsonRequestImpl<TRequest>(
            HttpMethod method,
            string url,
            TRequest request,
            JsonTypeInfo<TRequest> requestJsonInfo,
            CancellationToken cancellationToken
        ) where TRequest : MultipartDto
        {
            HttpResponseMessage? httpResponseMessage = null;
            try
            {
                var content = new MultipartFormDataContent();
                var jsonContent = JsonContent.Create(request, requestJsonInfo);
                jsonContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "payload_json"
                };

                content.Add(jsonContent);

                foreach (var attachment in request.Files ?? throw new InvalidOperationException())
                {
                    var partContent = attachment switch
                    {
                        StreamFileUpload streamed => (HttpContent)new StreamContent(streamed.Stream),
                        ArrayFileUpload array => new ByteArrayContent(array.Data),
                        _ => throw new ArgumentException("Unsupported file upload type", nameof(request)),
                    };
                    partContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = string.Format(CultureInfo.InvariantCulture, FileFormat, attachment.SendId),
                        FileName = attachment.FileName
                    };

                    if (attachment.ContentType is not null)
                    {
                        partContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
                    }

                    content.Add(partContent);
                }

                using var httpRequestMessage = new HttpRequestMessage(method, url);
                httpRequestMessage.Content = content;

                httpResponseMessage = await client.SendFluxerRequestAsync(httpRequestMessage, cancellationToken);
                return httpResponseMessage;
            }
            catch
            {
                httpResponseMessage?.Dispose();
                throw;
            }
        }

        public async Task<TResult> JsonRequestAsync<TRequest, TResult>(
            HttpMethod method,
            string url,
            TRequest request,
            JsonTypeInfo<TRequest> requestJsonInfo,
            JsonTypeInfo<TResult> resultJsonInfo,
            string? reason = null,
            CancellationToken cancellationToken = default
        )
            where TRequest : notnull
            where TResult : class
        {
            using var httpRequestMessage = new HttpRequestMessage(method, url);
            httpRequestMessage.Content = JsonContent.Create(request, requestJsonInfo);

            using var response = await client.SendFluxerRequestAsync(httpRequestMessage, cancellationToken, reason);

            return await HttpClient.DeserializeResponseAsync(resultJsonInfo, cancellationToken, response);
        }

        public async Task JsonRequestAsync<TRequest>(
            HttpMethod method,
            string url,
            TRequest request,
            JsonTypeInfo<TRequest> requestJsonInfo,
            string? reason = null,
            CancellationToken cancellationToken = default
        )
            where TRequest : notnull
        {
            using var httpRequestMessage = new HttpRequestMessage(method, url);
            httpRequestMessage.Content = JsonContent.Create(request, requestJsonInfo);

            using var response = await client.SendFluxerRequestAsync(httpRequestMessage, cancellationToken, reason);
        }

        public async Task<TResult> JsonRequestAsync<TResult>(
            HttpMethod method,
            string url,
            JsonTypeInfo<TResult> resultJsonInfo,
            string? reason = null,
            CancellationToken cancellationToken = default
        )
            where TResult : class
        {
            using var httpRequestMessage = new HttpRequestMessage(method, url);
            using var response = await client.SendFluxerRequestAsync(httpRequestMessage, cancellationToken, reason);

            return await HttpClient.DeserializeResponseAsync(resultJsonInfo, cancellationToken, response);
        }

        private static async Task<TResult> DeserializeResponseAsync<TResult>(
            JsonTypeInfo<TResult> resultJsonInfo,
            CancellationToken cancellationToken,
            HttpResponseMessage response) where TResult : class
        {
#if DEBUG
            var jsonString = await response
                .Content
                .ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize(jsonString, resultJsonInfo)
                   // this would only happen if the response is 'null' - which should never happen!!
                   ?? throw new ApiNullException();
#else
            return await response
                .Content
                .ReadFromJsonAsync<TResult>(
                    cancellationToken: cancellationToken,
                    resultJsonInfo
                ) ?? throw new ApiNullException();
#endif
        }

        public async Task RequestAsync(
            HttpMethod method,
            string url,
            string? reason = null,
            CancellationToken cancellationToken = default
        )
        {
            using var httpRequestMessage = new HttpRequestMessage(method, url);
            await client.SendFluxerRequestAsync(httpRequestMessage, cancellationToken, reason);
        }

        private async Task<HttpResponseMessage> SendFluxerRequestAsync(
            HttpRequestMessage httpRequestMessage,
            CancellationToken cancellationToken = default,
            string? reason = null
        )
        {
            httpRequestMessage.Version = client.DefaultRequestVersion;
            httpRequestMessage.VersionPolicy = client.DefaultVersionPolicy;

            if (reason is not null)
            {
                httpRequestMessage.Headers.Add("X-Audit-Log-Reason", reason);
            }

            var httpResponseMessage = await client.SendAsync(httpRequestMessage,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return httpResponseMessage;
            }

            try
            {
                var errorResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>(
                    RestDtoContext.Default.ErrorResponse,
                    cancellationToken: cancellationToken
                );

                if (httpResponseMessage.StatusCode != HttpStatusCode.TooManyRequests)
                {
                    throw new RestApiException(errorResponse!.Code, errorResponse.Message, errorResponse.Errors!);
                }
                else
                {
                    throw new RatelimitException(errorResponse!.Code, errorResponse.Message,
                        errorResponse.RetryAfter!.Value, errorResponse.Global!.Value);
                }
            }
            finally
            {
                httpResponseMessage.Dispose();
            }
        }
    }
}