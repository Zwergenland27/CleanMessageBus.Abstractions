namespace CleanMessageBus.Abstractions;

/// <summary>
/// The kind of an event.
/// </summary>
public enum EventKind
{
    /// <summary>
    /// An event implementing <see cref="IDomainEvent"/>.
    /// </summary>
    Domain,

    /// <summary>
    /// An event implementing <see cref="IIntegrationEvent"/>.
    /// </summary>
    Integration,
}