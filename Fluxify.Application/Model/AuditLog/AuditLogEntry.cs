using Fluxify.Dto.Guilds.AuditLog;

namespace Fluxify.Application.Model.AuditLog;

/// <summary>
/// An entry in the audit log
/// </summary>
/// <param name="Id">The unique id of the entry</param>
/// <param name="Type">The action type that was logged</param>
/// <param name="Changes">Changes that occurred because of the action</param>
/// <param name="Options">Additional metadata (dependent on the type)</param>
/// <param name="TargetId">The entity id on which the action was executed.</param>
/// <param name="Reason">The reason why the action was executed</param>
/// <param name="UserId">The id of the user that executed this action</param>
public record AuditLogEntry(
    Snowflake Id,
    AuditLogActionType Type,
    IAuditLogChange[]? Changes,
    Dictionary<string, string>? Options,
    string? TargetId,
    string? Reason,
    Snowflake? UserId
);