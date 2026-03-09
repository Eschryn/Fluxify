namespace Fluxify.Dto.Channels.Category;

public record ChannelCreateCategoryRequest(
    string Name,
    ChannelPermissionOverwrite[]? PermissionOverwrites
) : ChannelCreateRequest(
    Name,
    null,
    PermissionOverwrites);