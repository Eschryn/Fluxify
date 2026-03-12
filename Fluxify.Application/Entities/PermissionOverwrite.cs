using Fluxify.Core.Types;
using Fluxify.Dto.Channels;

namespace Fluxify.Application.Entities;

public class PermissionOverwrite
{
    public Snowflake Id { get; init; }
    public PermissionOverwriteType Type { get; init; }
    public Permissions Allow { get; init; }
    public Permissions Deny { get; init; }
}