namespace Fluxify.Dto.Reports;

public record DsaReportEmailVerifyRequest(
    string Email,
    string Token
);