namespace Fluxify.Dto.Admin;

public enum SuspiciousActivityFlags
{
    RequireVerifiedEmail = 1,
    RequireReverifiedEmail = 2,
    RequireVerifiedPhone = 4,
    RequireReverifiedPhone = 8,
    RequireVerifiedEmailOrVerifiedPhone = 16,
    RequireReverifiedEmailOrVerifiedPhone = 32,
    RequireVerifiedEmailOrReverifiedPhone = 64,
    RequireReverifiedEmailOrReverifiedPhone = 128,
}