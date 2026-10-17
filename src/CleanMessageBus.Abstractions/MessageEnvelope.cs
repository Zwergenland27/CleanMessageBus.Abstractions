namespace CleanMessageBus.Abstractions;

/// <summary>
/// Transport-neutral representation of a serialized event including its metadata.
/// </summary>
public sealed record MessageEnvelope
{
    private static readonly IReadOnlyDictionary<string, string> EmptyHeaders =
        new Dictionary<string, string>(StringComparer.Ordinal).AsReadOnly();

    /// <summary>
    /// Unique id of the message. Stays the same for redeliveries.
    /// </summary>
    public required Guid MessageId { get; init; }

    /// <summary>
    /// Name of the published event
    /// </summary>
    public required string EventName { get; init; }

    /// <summary>
    /// Whether the payload is a domain or an integration event.
    /// </summary>
    public required EventKind Kind { get; init; }

    /// <summary>
    /// Name of the application that published the event.
    /// </summary>
    public required string SourceApplication { get; init; }

    /// <summary>
    /// The serialized event.
    /// </summary>
    public required ReadOnlyMemory<byte> Body { get; init; }

    /// <summary>
    /// Content type of <see cref="Body"/>, e.g. <c>application/json</c>.
    /// </summary>
    public required string ContentType { get; init; }

    /// <summary>
    /// Point in time the event was published.
    /// </summary>
    public required DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// Additional headers such as trace context.
    /// </summary>
    public IReadOnlyDictionary<string, string> Headers { get; init; } = EmptyHeaders;
}