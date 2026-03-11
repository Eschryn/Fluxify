using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Fluxify.Dto;

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
            where TRequest : MultipartModel
            where TResult : class
        {
            if (request.Files is not { Length: > 0 })
            {
                return await client.JsonRequestAsync<TRequest, TResult>(method, url, request, options, cancellationToken);
            }

            var content = new MultipartFormDataContent();
            foreach (var attachment in request.Files)
            {
                content.Add(
                    new StreamContent(attachment.Data),
                    string.Format(CultureInfo.InvariantCulture, FileFormat, attachment.PlaceholderId),
                    attachment.FileName
                );
            }

            content.Add(JsonContent.Create(request, options: options), "payload_json");

            var httpRequestMessage = new HttpRequestMessage(method, url)
            {
                Version = client.DefaultRequestVersion,
                VersionPolicy = client.DefaultVersionPolicy,
                Content = content
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage,
                HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            return await httpResponseMessage
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken, options: options);
        }

        public async Task<TResult?> JsonRequestAsync<TRequest, TResult>(
            HttpMethod method,
            string url,
            TRequest request,
            JsonSerializerOptions options,
            CancellationToken cancellationToken = default
        )
            where TRequest : notnull
            where TResult : class
        {
            var httpRequestMessage = new HttpRequestMessage(method, url)
            {
                Version = client.DefaultRequestVersion,
                VersionPolicy = client.DefaultVersionPolicy,
                Content = JsonContent.Create(request, options: options)
            };

            var result = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            return await result.EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken, options: options);
        }
    }
}