namespace Fluxify.Dto.Users.Relationships;

public record FriendRequestByTagRequest(
    string Discriminator,
    string Username
);