namespace Fluxify.Dto.Packs;

public record PackDashboardResponse(
    PackDashboardResponseDetail Emoji,
    PackDashboardResponseDetail Sticker
);