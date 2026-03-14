using System.Globalization;
using System.Text;
using Fluxify.Dto.Users.Settings.Security;
using Fluxify.Dto.Users.Settings.Security.Mfa;
using Fluxify.Dto.Users.Settings.Security.Webauth;

namespace Fluxify.Rest.Users;

public class MfaRequestBuilder(HttpClient client)
{
    private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    private const string BackupCodesUrl = "users/@me/mfa/backup-codes";
    private const string SmsDisableUrl = "users/@me/mfa/sms/disable";
    private const string SmsEnableUrl = "users/@me/mfa/sms/enable";
    private const string TotpDisableUrl = "users/@me/mfa/totp/disable";
    private const string TotpEnableUrl = "users/@me/mfa/totp/enable";
    private const string WebAuthnCredentialsUrl = "users/@me/mfa/webauthn/credentials";
    private const string WebAuthnCredentialsRegistrationOptionsUrl = "users/@me/mfa/webauthn/credentials/registration-options";
    private static readonly CompositeFormat WebAuthnCredentialUrl = CompositeFormat.Parse("users/@me/mfa/webauthn/credentials/{0}");
    
    public async Task<MfaBackupCodesResponse?> GetBackupCodes(
        MfaBackupCodesRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<MfaBackupCodesRequest, MfaBackupCodesResponse>(
        HttpMethod.Post,
        BackupCodesUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task DisableSmsAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        SmsDisableUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task EnableSmsAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        SmsEnableUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task DisableTotpAsync(
        DisableTotpRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        TotpDisableUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<MfaBackupCodesResponse?> EnableTotpAsync(
        EnableMfaTotpRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<EnableMfaTotpRequest, MfaBackupCodesResponse>(
        HttpMethod.Post,
        TotpEnableUrl,
        request,
        cancellationToken: cancellationToken
    );

    public async Task<WebAuthnCredentialsResponse?> ListWebAuthnCredentialsAsync(
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<WebAuthnCredentialsResponse>(
        HttpMethod.Get,
        WebAuthnCredentialsUrl,
        cancellationToken: cancellationToken
    );

    public async Task RegisterWebAuthnCredentialAsync(
        WebAuthnRegisterRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        WebAuthnCredentialsUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<WebAuthnChallengeResponse?> GetWebAuthnRegistrationOptionsAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<SudoVerificationSchema, WebAuthnChallengeResponse>(
        HttpMethod.Post,
        WebAuthnCredentialsRegistrationOptionsUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task DeleteWebAuthnCredentialAsync(
        string credentialId,
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, WebAuthnCredentialUrl, credentialId),
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task UpdateWebAuthnCredentialAsync(
        string credentialId,
        WebAuthnCredentialUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Patch,
        string.Format(FormatProvider, WebAuthnCredentialUrl, credentialId),
        request,
        cancellationToken: cancellationToken
    );
}