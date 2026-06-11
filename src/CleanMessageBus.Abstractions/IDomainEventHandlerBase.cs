using CleanDomainValidation.Domain;

namespace CleanMessageBus.Abstractions;

/// <summary>
/// Defines handler for events of type <typeparamref name="TDomainEvent"/>
/// </summary>
public abstract class DomainEventHandlerBase<TDomainEvent> : IRequestHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{
    /// <summary>
    /// The max. amount of events that can be polled from the broker to the active instance simultaneously
    /// </summary>
    public uint MaxQueueSize { get; private init; }
    
    /// <summary>
    /// Initializes the handler with a specific queue size
    /// </summary>
    /// <param name="maxQueueSize">The max. amount of events that can be polled from the broker to the active instance simultaneously</param>
    protected DomainEventHandlerBase(uint maxQueueSize)
    {
        MaxQueueSize = maxQueueSize;
    }
    
    /// <summary>
    /// Initializes the handler without a specific queue size
    /// </summary>
    protected DomainEventHandlerBase()
    {
        MaxQueueSize = uint.MaxValue;
    }
    
    /// <summary>
    /// Actual event handler logic
    /// </summary>
    /// <param name="event">Incoming event object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public abstract Task<CanFail> Handle(TDomainEvent @event, CancellationToken cancellationToken);
}