using Fluxify.Core.Types;
using Fluxify.Dto.Guilds.Settings;

namespace Fluxify.Dto.Users.Settings;

public record UserSettingsResponseGuildFoldersItem(
    int? Color,
    GuildFolderFlags? Flags,
    Snowflake[] GuildIds,
    // UserSettingsResponseGuildFoldersItemIcon? Icon, // Schema is not available more research needed TODO
    int? Id,
    string? Name
);