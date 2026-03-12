using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Fluxify.Dto;
using Fluxify.Dto.Uploads;
using Fluxify.Rest.Model;

namespace Fluxify.Rest;

internal static class HttpClientExtensions
{
    private static readonly CompositeFormat FileFormat = CompositeFormat.Parse("files[{0}]");

    extension(HttpClient client)
    {
        public async Task<TResult?> MultipartJsonRequestAsync<TRequest, TResult>(
            HttpMethod method,
            string url,
            TRequest request,
            JsonSerializerOptions options,
            CancellationToken cancellationToken = default)
            where TRequest : MultipartDto
            where TResult : class
        {
            if (request.Files is not { Length: > 0 })
            {
                return await client.JsonRequestAsync<TRequest, TResult>(method, url, request, options, null, cancellationToken);
            }

            var content = new MultipartFormDataContent();
            var jsonContent = JsonContent.Create(request, options: options);
            jsonContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "payload_json"
            };
            
            content.Add(jsonContent);
            
            foreach (var attachment in request.Files)
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
                partContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType.ToString());
                
                content.Add(partContent);
            }
            
            using var httpRequestMessage = new HttpRequestMessage(method, url);
            httpRequestMessage.Version = client.DefaultRequestVersion;
            httpRequestMessage.VersionPolicy = client.DefaultVersionPolicy;
            httpRequestMessage.Content = content;

            using var httpResponseMessage = await client.SendAsync(httpRequestMessage,
                HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage
                    .Content
                    .ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken, options: options);
            }

            var errorResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>(
                options: options,
                cancellationToken: cancellationToken
            );

            if (httpResponseMessage.StatusCode != HttpStatusCode.TooManyRequests)
            {
                throw new RestApiException(errorResponse!.Code, errorResponse.Message, errorResponse.Errors!);
            }
            else
            {
                throw new RatelimitException(errorResponse!.Code, errorResponse.Message, errorResponse.RetryAfter!.Value, errorResponse.Global!.Value);
            }
        }

        public async Task<TResult?> JsonRequestAsync<TRequest, TResult>(
            HttpMethod method,
            string url,
            TRequest request,
            JsonSerializerOptions options,
            string? reason = null,
            CancellationToken cancellationToken = default
        )
            where TRequest : notnull
            where TResult : class
        {
            using var httpRequestMessage = new HttpRequestMessage(method, url);
            httpRequestMessage.Version = client.DefaultRequestVersion;
            httpRequestMessage.VersionPolicy = client.DefaultVersionPolicy;
            httpRequestMessage.Content = JsonContent.Create(request, options: options);

            if (reason is not null)
            {
                httpRequestMessage.Headers.Add("X-Audit-Log-Reason", reason);
            }

            using var httpResponseMessage = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage
                    .Content
                    .ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken, options: options);
            }

            var errorResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>(
                options: options,
                cancellationToken: cancellationToken
            );

            if (httpResponseMessage.StatusCode != HttpStatusCode.TooManyRequests)
            {
                throw new RestApiException(errorResponse!.Code, errorResponse.Message, errorResponse.Errors!);
            }
            else
            {
                throw new RatelimitException(errorResponse!.Code, errorResponse.Message, errorResponse.RetryAfter!.Value, errorResponse.Global!.Value);
            }
        }
    }
}