using CleanDomainValidation.Application;

namespace CleanMessageBus.Abstractions;

/// <summary>
/// Integration event
/// </summary>
public abstract class IntegrationEvent: IRequest
{
    /// <summary>
    /// Id of the event
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Creates a new integration event
    /// </summary>
    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
    }
}