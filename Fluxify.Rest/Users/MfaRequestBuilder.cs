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
    
    public Task<MfaBackupCodesResponse> GetBackupCodes(
        MfaBackupCodesRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<MfaBackupCodesRequest, MfaBackupCodesResponse>(
        HttpMethod.Post,
        BackupCodesUrl,
        request,
        DtoJsonContext.Default.MfaBackupCodesRequest,
        DtoJsonContext.Default.MfaBackupCodesResponse,
        cancellationToken: cancellationToken
    );
    
    public Task DisableSmsAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        SmsDisableUrl,
        request,
        DtoJsonContext.Default.SudoVerificationSchema,
        cancellationToken: cancellationToken
    );
    
    public Task EnableSmsAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        SmsEnableUrl,
        request,
        DtoJsonContext.Default.SudoVerificationSchema,
        cancellationToken: cancellationToken
    );
    
    public Task DisableTotpAsync(
        DisableTotpRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        TotpDisableUrl,
        request,
        DtoJsonContext.Default.DisableTotpRequest,
        cancellationToken: cancellationToken
    );
    
    public Task<MfaBackupCodesResponse> EnableTotpAsync(
        EnableMfaTotpRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<EnableMfaTotpRequest, MfaBackupCodesResponse>(
        HttpMethod.Post,
        TotpEnableUrl,
        request,
        DtoJsonContext.Default.EnableMfaTotpRequest,
        DtoJsonContext.Default.MfaBackupCodesResponse,
        cancellationToken: cancellationToken
    );

    public Task<WebAuthnCredentialsResponse> ListWebAuthnCredentialsAsync(
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<WebAuthnCredentialsResponse>(
        HttpMethod.Get,
        WebAuthnCredentialsUrl,
        DtoJsonContext.Default.WebAuthnCredentialsResponse,
        cancellationToken: cancellationToken
    );

    public Task RegisterWebAuthnCredentialAsync(
        WebAuthnRegisterRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Post,
        WebAuthnCredentialsUrl,
        request,
        DtoJsonContext.Default.WebAuthnRegisterRequest,
        cancellationToken: cancellationToken
    );
    
    public Task<WebAuthnChallengeResponse> GetWebAuthnRegistrationOptionsAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync<SudoVerificationSchema, WebAuthnChallengeResponse>(
        HttpMethod.Post,
        WebAuthnCredentialsRegistrationOptionsUrl,
        request,
        DtoJsonContext.Default.SudoVerificationSchema,
        DtoJsonContext.Default.WebAuthnChallengeResponse,
        cancellationToken: cancellationToken
    );
    
    public Task DeleteWebAuthnCredentialAsync(
        string credentialId,
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Delete,
        string.Format(FormatProvider, WebAuthnCredentialUrl, credentialId),
        request,
        DtoJsonContext.Default.SudoVerificationSchema,
        cancellationToken: cancellationToken
    );
    
    public Task UpdateWebAuthnCredentialAsync(
        string credentialId,
        WebAuthnCredentialUpdateRequest request,
        CancellationToken cancellationToken = default
    ) => client.JsonRequestAsync(
        HttpMethod.Patch,
        string.Format(FormatProvider, WebAuthnCredentialUrl, credentialId),
        request,
        DtoJsonContext.Default.WebAuthnCredentialUpdateRequest,
        cancellationToken: cancellationToken
    );
}