using System.Text.Json.Serialization;

namespace Fluxify.Dto;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MfaMethod
{
    Totp = 1,
    Sms = 2,
    Webauthn = 3
}