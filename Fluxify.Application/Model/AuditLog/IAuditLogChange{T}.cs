namespace Fluxify.Application.Model.AuditLog;

/// <inheritdoc/>
/// <typeparam name="TValue">Type of the property</typeparam>
public interface IAuditLogChange<out TValue> : IAuditLogChange
{
    /// <inheritdoc cref="IAuditLogChange.OldValue"/>
    new TValue? OldValue { get; }
    /// <inheritdoc cref="IAuditLogChange.OldValue"/>
    new TValue? NewValue { get; }

    object? IAuditLogChange.OldValue => OldValue;
    object? IAuditLogChange.NewValue => NewValue;
}