namespace CleanMessageBus.Abstractions;

/// <summary>
/// Marks an event that belongs to the application that raises it.
/// Domain events are published to the application's own domain exchange and are only
/// handled by the application itself.
/// </summary>
public interface IDomainEvent;