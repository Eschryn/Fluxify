using Fluxify.Core.Types;

namespace Fluxify.Application.Entities.Users;

public class PartialUser : IEntity
{
    public Snowflake Id { get; init; }
    public bool? Bot { get; init; }
    public string? Username { get; init; }
    public string? Discriminator { get; init; }
    public string? GlobalName { get; init; }
    public string? Avatar { get; init; }
    public int? AvatarColor { get; init; }
    public bool? System { get; init; }
    public PublicUserFlags Flags { get; init; }
}