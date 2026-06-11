using CleanDomainValidation.Application;

namespace CleanMessageBus.Abstractions;

/// <summary>
/// Domain event
/// </summary>
public abstract class DomainEvent: IRequest
{
    /// <summary>
    /// Id of the event
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Creates a new domain event
    /// </summary>
    public DomainEvent()
    {
        Id = Guid.NewGuid();
    }
}