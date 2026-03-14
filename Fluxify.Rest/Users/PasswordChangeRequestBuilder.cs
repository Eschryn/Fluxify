using Fluxify.Dto.Users.Settings.PasswordChange;

namespace Fluxify.Rest.Users;

public class PasswordChangeRequestBuilder(HttpClient client)
{
    private const string CompleteUrl = "users/@me/password-change/complete";
    private const string ResendUrl = "users/@me/password-change/resend";
    private const string StartUrl = "users/@me/password-change/start";
    private const string VerifyUrl = "users/@me/password-change/verify";
    
    public async Task CompleteAsync(
        PasswordChangeCompleteRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        CompleteUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task ResendAsync(
        PasswordChangeTicketRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        ResendUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<PasswordChangeStartResponse?> StartAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<PasswordChangeStartResponse>(
        HttpMethod.Post,
        StartUrl,
        cancellationToken: cancellationToken
    );

    public async Task<PasswordChangeVerifyResponse?> VerifyAsync(
        PasswordChangeVerifyRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<PasswordChangeVerifyRequest, PasswordChangeVerifyResponse>(
        HttpMethod.Post,
        VerifyUrl,
        request,
        cancellationToken: cancellationToken
    );
}