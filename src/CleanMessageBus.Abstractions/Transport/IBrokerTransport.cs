namespace CleanMessageBus.Abstractions.Transport;

/// <summary>
/// Connects CleanMessageBus to a message broker.
/// </summary>
public interface IBrokerTransport
{
    /// <summary>
    /// Name of the messaging system, used as <c>messaging.system</c> telemetry attribute (e.g. <c>rabbitmq</c>).
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Declares the consumer topology (queues, bindings, dead-letter destinations) and starts consuming.
    /// Called once at application start; if it throws it is called again with exponential backoff. Implementations are responsible for reconnecting afterwards.
    /// </summary>
    Task StartAsync(TransportStartContext context, CancellationToken cancellationToken);

    /// <summary>
    /// Stops consuming. In-flight messages should be completed or returned to the broker.
    /// </summary>
    Task StopAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Publishes an envelope. Publish destinations (e.g. exchanges) are derived from the envelope and
    /// declared on demand, so event types never have to be registered up front.
    /// </summary>
    ValueTask PublishAsync(MessageEnvelope envelope, CancellationToken cancellationToken);
}