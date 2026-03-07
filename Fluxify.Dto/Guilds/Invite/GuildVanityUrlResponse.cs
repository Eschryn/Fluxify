namespace Fluxify.Dto.Guilds.Invite;

public record GuildVanityUrlResponse(
    string? Code,
    int Uses
);