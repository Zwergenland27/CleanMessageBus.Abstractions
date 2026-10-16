namespace CleanMessageBus.Abstractions.Transport;

/// <summary>
/// Everything a transport needs to declare its topology and start consuming.
/// </summary>
public sealed class TransportStartContext
{
    /// <summary>
    /// Creates the context.
    /// </summary>
    public TransportStartContext(
        string applicationName,
        IReadOnlyList<EventHandlerDescriptor> handlers,
        IMessageReceiver receiver)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        ArgumentNullException.ThrowIfNull(handlers);
        ArgumentNullException.ThrowIfNull(receiver);

        ApplicationName = applicationName;
        Handlers = handlers;
        Receiver = receiver;
    }

    /// <summary>
    /// Name of the current application.
    /// </summary>
    public string ApplicationName { get; }

    /// <summary>
    /// All mapped handlers.
    /// </summary>
    public IReadOnlyList<EventHandlerDescriptor> Handlers { get; }

    /// <summary>
    /// Receiver to pass consumed messages to.
    /// </summary>
    public IMessageReceiver Receiver { get; }
}