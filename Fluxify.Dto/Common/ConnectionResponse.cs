using Fluxify.Core;

namespace Fluxify.Dto.Common;

public record ConnectionResponse(
    Snowflake Id,
    string Name,
    int SortOrder,
    ConnectionType Type,
    bool Verified,
    int VisibilityFlags // TODO: What is this?
    );