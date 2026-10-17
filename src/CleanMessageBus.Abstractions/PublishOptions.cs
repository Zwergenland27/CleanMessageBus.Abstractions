namespace CleanMessageBus.Abstractions;

/// <summary>
/// Metadata for publishing an event that was raised earlier, typically by an outbox worker.
/// </summary>
public sealed record PublishOptions
{
    /// <summary>
    /// Message id to use, e.g. the id of the outbox entry. Publishing the same entry twice then yields the same
    /// <see cref="MessageEnvelope.MessageId"/>, so consumers can detect duplicates. Defaults to a new id.
    /// </summary>
    public Guid? MessageId { get; init; }

    /// <summary>
    /// When the event was raised. Defaults to the time of publishing.
    /// </summary>
    public DateTimeOffset? Timestamp { get; init; }

    /// <summary>
    /// Trace context captured when the event was raised.
    /// When options are passed, this is the only parent of the send span: the activity of the publishing component
    /// (e.g. the outbox worker) is ignored. Without a trace context the message starts a new trace.
    /// </summary>
    public MessageTraceContext? TraceContext { get; init; }
}