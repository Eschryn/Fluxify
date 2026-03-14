using Fluxify.Core.Types;

namespace Fluxify.Dto.Users;

public record UserPartialResponse(
    string? Avatar,
    int? AvatarColor,
    bool? Bot,
    string Discriminator,
    PublicUserFlags Flags,
    string? GlobalName,
    Snowflake Id,
    bool? System,
    string Username);