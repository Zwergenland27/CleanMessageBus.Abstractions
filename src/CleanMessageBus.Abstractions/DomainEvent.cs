namespace CleanMessageBus.Abstractions;

/// <summary>
/// Domain event
/// </summary>
public abstract class DomainEvent: IDomainEvent
{
    /// <inheritdoc/>
    public Guid Id { get; }

    /// <summary>
    /// Creates a new domain event
    /// </summary>
    public DomainEvent()
    {
        Id = Guid.NewGuid();
    }
}