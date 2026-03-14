using Fluxify.Dto.Users.Settings.Security.Mfa;
using Fluxify.Dto.Users.Settings.Security.Webauth;

namespace Fluxify.Rest.Users;

public class SudoRequestBuilder(HttpClient client)
{
    private const string MfaMethodsUrl = "users/@me/sudo/mfa-methods";
    private const string SmsSendUrl = "users/@me/sudo/sms/send";
    private const string WebAuthnOptionsUrl = "users/@me/sudo/webauthn/authentication-options";
    
    public async Task<SudoMfaMethodsResponse?> GetMfaMethodsAsync(CancellationToken cancellationToken = default)
        => await client.JsonRequestAsync<SudoMfaMethodsResponse>(
            HttpMethod.Get,
            MfaMethodsUrl,
            cancellationToken: cancellationToken
        );
    
    public async Task SendSmsCodeAsync(
        CancellationToken cancellationToken = default
    ) => await client.RequestAsync(
        HttpMethod.Post,
        SmsSendUrl,
        cancellationToken: cancellationToken
    );
    
    public async Task<WebAuthnChallengeResponse?> GetWebAuthnOptionsAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<WebAuthnChallengeResponse>(
        HttpMethod.Post,
        WebAuthnOptionsUrl,
        cancellationToken: cancellationToken
    );
}