using Fluxify.Core;
using Fluxify.Dto.Channels.LinkChannel;

namespace Fluxify.Dto.Channels.Category;

public record ChannelUpdateCategoryRequest(
    string? Name,
    ChannelPermissionOverwrite[] PermissionOverwrites
) : ChannelUpdateRequest;