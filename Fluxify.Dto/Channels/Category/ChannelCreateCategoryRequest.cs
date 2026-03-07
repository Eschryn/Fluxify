namespace Fluxify.Dto.Channels.Category;

public record ChannelCreateCategoryRequest(
    string Name,
    ChannelPermissionOverwrite[]? PermissionOverwrites
) : ChannelCreateRequest(
    Name,
    ChannelType.Category,
    null,
    PermissionOverwrites);