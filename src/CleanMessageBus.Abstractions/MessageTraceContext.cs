using System.Diagnostics;

namespace CleanMessageBus.Abstractions;

/// <summary>
/// Serializable W3C trace context (<c>traceparent</c>, <c>tracestate</c>, <c>baggage</c>).
/// </summary>
/// <remarks>
/// Capture it with <see cref="Capture()"/> when an event is written to an outbox, store it next to the event and pass it
/// to <see cref="IEventPublisher"/> via <see cref="PublishOptions.TraceContext"/> when the outbox is processed.
/// This keeps the trace of the code that raised the event, even though the event is published later by another component.
/// </remarks>
public sealed record MessageTraceContext
{
    private static readonly DistributedContextPropagator Propagator =
        DistributedContextPropagator.CreateW3CPropagator();

    
    /// <summary>
    /// Creates a trace context.
    /// </summary>
    /// <param name="traceParent">W3C <c>traceparent</c>, e.g. <c>00-{traceId}-{spanId}-01</c>.</param>
    /// <param name="traceState">W3C <c>tracestate</c>, if any.</param>
    /// <param name="baggage">W3C <c>baggage</c>, if any.</param>
    public MessageTraceContext(string traceParent, string? traceState = null, string? baggage = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(traceParent);

        TraceParent = traceParent;
        TraceState = string.IsNullOrEmpty(traceState) ? null : traceState;
        Baggage = string.IsNullOrEmpty(baggage) ? null : baggage;
    }

    /// <summary>
    /// W3C <c>traceparent</c>.
    /// </summary>
    public string TraceParent { get; }

    /// <summary>
    /// W3C <c>tracestate</c>.
    /// </summary>
    public string? TraceState { get; }

    /// <summary>
    /// W3C <c>baggage</c>.
    /// </summary>
    public string? Baggage { get; }

    /// <summary>
    /// Captures the context of <see cref="Activity.Current"/>, or <c>null</c> if there is none.
    /// </summary>
    public static MessageTraceContext? Capture() => Capture(Activity.Current);

    /// <summary>
    /// Captures the context of <paramref name="activity"/>, or <c>null</c> if it is missing or not W3C.
    /// </summary>
    public static MessageTraceContext? Capture(Activity? activity)
    {
        if (activity is not {IdFormat: ActivityIdFormat.W3C})
        {
            return null;
        }
        
        var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        Propagator.Inject(activity, fields, static (carrier, name, value) =>
        {
            if (!string.IsNullOrEmpty(value))
            {
                ((Dictionary<string, string>)carrier!)[name] = value;
            }
        });

        if (!fields.TryGetValue(MessageHeaderNames.TraceParent, out var traceParent))
        {
            return null;
        }

        return new MessageTraceContext(
            traceParent,
            fields.GetValueOrDefault(MessageHeaderNames.TraceState),
            fields.GetValueOrDefault(MessageHeaderNames.Baggage));
    }

    /// <summary>
    /// Reads the context from message headers, or returns <c>null</c> if they contain none.
    /// </summary>
    public static MessageTraceContext? FromHeaders(IReadOnlyDictionary<string, string> headers)
    {
        ArgumentNullException.ThrowIfNull(headers);

        if (!headers.TryGetValue(MessageHeaderNames.TraceParent, out var traceParent) || string.IsNullOrWhiteSpace(traceParent))
        {
            return null;
        }

        return new MessageTraceContext(
            traceParent,
            headers.GetValueOrDefault(MessageHeaderNames.TraceState),
            headers.GetValueOrDefault(MessageHeaderNames.Baggage));
    }

    /// <summary>
    /// Writes the context into message headers.
    /// </summary>
    public void WriteTo(IDictionary<string, string> headers)
    {
        ArgumentNullException.ThrowIfNull(headers);

        headers[MessageHeaderNames.TraceParent] = TraceParent;
        if (TraceState is not null)
        {
            headers[MessageHeaderNames.TraceState] = TraceState;
        }

        if (Baggage is not null)
        {
            headers[MessageHeaderNames.Baggage] = Baggage;
        }
    }

    /// <summary>
    /// Parses the context into an <see cref="ActivityContext"/> to be used as parent.
    /// </summary>
    public bool TryGetParent(out ActivityContext parent) =>
        ActivityContext.TryParse(TraceParent, TraceState, isRemote: true, out parent);

    /// <summary>
    /// Returns the decoded baggage entries.
    /// </summary>
    public IEnumerable<KeyValuePair<string, string?>> GetBaggage()
    {
        if (Baggage is null) return [];
        
        return Propagator.ExtractBaggage(this, static (carrier, name,
            out value, out values) =>
        {
            values = null;
            var self = (MessageTraceContext)carrier!;
            value = string.Equals(name, MessageHeaderNames.Baggage, StringComparison.OrdinalIgnoreCase)
                ? self.Baggage
                : null;
        }) ?? [];
    }
}