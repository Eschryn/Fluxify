using Fluxify.Core;

namespace Fluxify.Dto.Guilds.Settings;

public record GuildTransferOwnershipRequest(
    Snowflake NewOwnerId,
    string Password
);