namespace CleanMessageBus.Abstractions;

/// <summary>
/// Publishes events.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes a domain event of the current application, continuing the trace of <c>Activity.Current</c>.
    /// </summary>
    ValueTask PublishDomainEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;

    /// <summary>
    /// Publishes a domain event that was raised earlier (e.g. from an outbox), using the stored message id,
    /// timestamp and trace context.
    /// </summary>
    ValueTask PublishDomainEventAsync<TEvent>(TEvent @event, PublishOptions options, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;

    /// <summary>
    /// Publishes an integration event of the current application, continuing the trace of <c>Activity.Current</c>.
    /// </summary>
    ValueTask PublishIntegrationEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent;

    /// <summary>
    /// Publishes an integration event that was raised earlier (e.g. from an outbox), using the stored message id,
    /// timestamp and trace context.
    /// </summary>
    ValueTask PublishIntegrationEventAsync<TEvent>(TEvent @event, PublishOptions options, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent;
}