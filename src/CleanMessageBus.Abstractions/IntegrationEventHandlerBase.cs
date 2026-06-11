using CleanDomainValidation.Domain;

namespace CleanMessageBus.Abstractions;

/// <summary>
/// Defines handler for events of type <typeparamref name="TIntegrationEvent"/>
/// </summary>
public abstract class IntegrationEventHandlerBase<TIntegrationEvent> : IRequestHandler<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// The max. amount of events that can be polled from the broker to the active instance simultaneously
    /// </summary>
    public uint MaxQueueSize { get; private init; }
    
    /// <summary>
    /// Name of the producer
    /// </summary>
    public string ProducerName { get; private init; }
    
    /// <summary>
    /// Initializes the handler with a specific queue size
    /// </summary>
    /// <param name="producerName">Name of the producer</param>
    /// <param name="maxQueueSize">The max. amount of events that can be polled from the broker to the active instance simultaneously</param>
    protected IntegrationEventHandlerBase(string producerName, uint maxQueueSize)
    {
        ProducerName = producerName;
        MaxQueueSize = maxQueueSize;
    }
    
    /// <summary>
    /// Initializes the handler without a specific queue size
    /// </summary>
    /// <param name="producerName">Name of the producer</param>
    protected IntegrationEventHandlerBase(string producerName)
    {
        ProducerName = producerName;
        MaxQueueSize = uint.MaxValue;
    }
    
    /// <summary>
    /// Actual event handler logic
    /// </summary>
    /// <param name="event">Incoming event object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public abstract Task<CanFail> Handle(TIntegrationEvent @event, CancellationToken cancellationToken);
}