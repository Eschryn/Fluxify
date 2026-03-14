using Fluxify.Dto.Common;
using Fluxify.Dto.Users.Settings.Security;

namespace Fluxify.Rest.Users;

public class UserMessagesRequestBuilder(HttpClient client)
{
    private const string DeleteUrl = "users/@me/messages/delete";
    private const string DeleteTestUrl = "users/@me/messages/delete/test";

    public async Task DeleteBulkAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        DeleteUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<bool?> CancelBulkDeleteAsync(
        CancellationToken cancellationToken = default
    ) => (await client.JsonRequestAsync<SuccessResponse>(
        HttpMethod.Delete,
        DeleteUrl,
        cancellationToken: cancellationToken
    ))?.Success;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <remarks>This endpoint is staff only!</remarks>
    public async Task TestDeleteBulkAsync(
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Post,
        DeleteTestUrl,
        cancellationToken: cancellationToken
    );
}