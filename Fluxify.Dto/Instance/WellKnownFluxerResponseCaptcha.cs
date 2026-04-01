namespace Fluxify.Dto.Instance;

public record WellKnownFluxerResponseCaptcha(
    string? HcaptchaSiteKey,
    string? TurnstileSiteKey,
    CaptchaProvider Provider
);