namespace Fluxify.Dto.Guilds.AuditLog;



// GUILDS ARE DONE!!

/// <summary>
/// Represents a change in an audit log entry
/// </summary>
/// <param name="Key">The field that changed</param>
/// <param name="OldValue">Value before change</param>
/// <param name="NewValue">Value after change</param>
public record AuditLogChangeSchema(
    string Key,
    object? OldValue,
    object? NewValue);