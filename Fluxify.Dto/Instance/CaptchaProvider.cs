using System.Text.Json.Serialization;

namespace Fluxify.Dto.Instance;

[JsonConverter(typeof(JsonStringEnumConverter<CaptchaProvider>))]
public enum CaptchaProvider
{
    [JsonStringEnumMemberName("hcaptcha")] HCaptcha,

    [JsonStringEnumMemberName("turnstile")]
    Turnstile,
    [JsonStringEnumMemberName("none")] None
}