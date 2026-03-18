namespace Fluxify.Dto.Packs;

public record PackDashboardResponseDetail(
    PackSummaryResponse[] Created,
    int CreatedLimit,
    PackSummaryResponse[] Installed,
    int InstalledLimit
);