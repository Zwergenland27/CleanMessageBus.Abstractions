namespace CleanMessageBus.Abstractions;

/// <summary>
/// Describes a mapped event handler. Transports derive queues/subscriptions from it.
/// </summary>
public sealed class EventHandlerDescriptor
{
    private static readonly IReadOnlyDictionary<string, object?> EmptyProperties =
        new Dictionary<string, object?>(StringComparer.Ordinal).AsReadOnly();

    /// <summary>
    /// Creates a descriptor.
    /// </summary>
    public EventHandlerDescriptor(
        string name,
        Type eventType,
        string eventName,
        EventKind kind,
        string sourceApplication,
        int prefetchCount,
        RetryPolicy retryPolicy,
        IReadOnlyDictionary<string, object?>? properties = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(eventType);
        ArgumentException.ThrowIfNullOrWhiteSpace(eventName);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceApplication);
        ArgumentOutOfRangeException.ThrowIfLessThan(prefetchCount, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(prefetchCount, MaxPrefetchCount);
        ArgumentNullException.ThrowIfNull(retryPolicy);

        Name = name;
        EventType = eventType;
        EventName = eventName;
        Kind = kind;
        SourceApplication = sourceApplication;
        PrefetchCount = prefetchCount;
        RetryPolicy = retryPolicy;
        Properties = properties is null || properties.Count == 0
            ? EmptyProperties
            : new Dictionary<string, object?>(properties, StringComparer.Ordinal).AsReadOnly();
    }

    /// <summary>
    /// Name of the handler, unique within the application. Transports use it for queue names.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// CLR type of the handled event.
    /// </summary>
    public Type EventType { get; }

    /// <summary>
    /// Name of the handled event.
    /// </summary>
    public string EventName { get; }

    /// <summary>
    /// Kind of the handled event.
    /// </summary>
    public EventKind Kind { get; }

    /// <summary>
    /// Application that publishes the event. For domain events this is always the current application.
    /// </summary>
    public string SourceApplication { get; }

    /// <summary>
    /// Largest supported <see cref="PrefetchCount"/> (limit of the messaging protocol).
    /// </summary>
    public const int MaxPrefetchCount = ushort.MaxValue;

    /// <summary>
    /// Maximum number of unacknowledged messages the transport fetches for this handler (per application instance).
    /// With 1, the next message is only fetched after the previous one has been processed; all other messages stay
    /// in the broker and can be consumed by other instances. Messages of one handler are always processed sequentially.
    /// </summary>
    public int PrefetchCount { get; }

    /// <summary>
    /// Retry behavior for failed messages.
    /// </summary>
    public RetryPolicy RetryPolicy { get; }

    /// <summary>
    /// Transport-specific settings, e.g. set by extension methods of a transport package.
    /// </summary>
    public IReadOnlyDictionary<string, object?> Properties { get; }
}