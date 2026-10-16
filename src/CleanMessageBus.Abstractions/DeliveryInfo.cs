namespace CleanMessageBus.Abstractions;

/// <summary>
/// Information about the current delivery of a message to a handler.
/// </summary>
public readonly record struct DeliveryInfo
{
    /// <summary>
    /// Creates delivery information.
    /// </summary>
    /// <param name="attempt">The 1-based delivery attempt.</param>
    public DeliveryInfo(int attempt)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(attempt, 1);
        Attempt = attempt;
    }

    /// <summary>
    /// The 1-based delivery attempt. Attempts caused by <see cref="RetryAfterException"/> are not counted.
    /// </summary>
    public int Attempt { get; }

    /// <summary>
    /// Whether the message has been delivered before.
    /// </summary>
    public bool IsRedelivery => Attempt > 1;
}