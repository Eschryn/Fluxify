namespace Fluxify.Dto.Users.Settings;

public record UserTagCheckRequest(
    string Username,
    string Discriminator
);