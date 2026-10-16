namespace CleanMessageBus.Abstractions;

/// <summary>
/// Well-known header names written by CleanMessageBus.
/// </summary>
public static class MessageHeaderNames
{
    /// <summary>
    /// W3C trace context: <c>traceparent</c>.
    /// </summary>
    public const string TraceParent = "traceparent";

    /// <summary>
    /// W3C trace context: <c>tracestate</c>.
    /// </summary>
    public const string TraceState = "tracestate";

    /// <summary>
    /// W3C baggage.
    /// </summary>
    public const string Baggage = "baggage";
}