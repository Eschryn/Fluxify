using Fluxify.Dto.Users.Settings.PhoneChange;
using Fluxify.Dto.Users.Settings.Security;

namespace Fluxify.Rest.Users;

public class PhoneRequestBuilder(HttpClient client)
{
    private const string PhoneUrl = "users/@me/phone";
    private const string SendVerificationUrl = "users/@me/phone/send-verification";
    private const string VerifyUrl = "users/@me/phone/verify";
    
    public async Task AddPhoneNumberAsync(
        PhoneAddRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        PhoneUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task DeletePhoneNumberAsync(
        SudoVerificationSchema request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Delete,
        PhoneUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task SendVerificationAsync(
        PhoneSendVerificationRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync(
        HttpMethod.Post,
        SendVerificationUrl,
        request,
        cancellationToken: cancellationToken
    );
    
    public async Task<PhoneVerifyResponse?> VerifyAsync(
        PhoneVerifyRequest request,
        CancellationToken cancellationToken = default
    ) => await client.JsonRequestAsync<PhoneVerifyRequest, PhoneVerifyResponse>(
        HttpMethod.Post,
        VerifyUrl,
        request,
        cancellationToken: cancellationToken
    );
}