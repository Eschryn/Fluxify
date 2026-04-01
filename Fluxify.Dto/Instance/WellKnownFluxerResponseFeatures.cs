namespace Fluxify.Dto.Instance;

public record WellKnownFluxerResponseFeatures(
    bool SmsMfaEnabled,
    bool VoiceEnabled,
    bool StripeEnabled,
    bool SelfHosted,
    bool PresignedAttachmentUploads
);