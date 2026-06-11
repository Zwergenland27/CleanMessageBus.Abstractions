namespace CleanMessageBus.Abstractions;

/// <summary>
/// Integration event
/// </summary>
public abstract class IntegrationEvent: IIntegrationEvent
{
    /// <inheritdoc/>
    public Guid Id { get; }

    /// <inheritdoc/>
    public string EventName { get; }

    /// <summary>
    /// Creates a new integration event
    /// </summary>
    /// <param name="name">Public name of the event</param>
    protected IntegrationEvent(string name)
    {
        Id = Guid.NewGuid();
        EventName = name;
    }
}