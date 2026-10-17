namespace CleanMessageBus.Abstractions;

/// <summary>
/// Marks an event that crosses application boundaries.
/// Integration events are published by one application and may be handled by any other application.
/// </summary>
public interface IIntegrationEvent;