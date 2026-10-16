namespace CleanMessageBus.Abstractions;

/// <summary>
/// Result of processing a message, interpreted by the transport.
/// </summary>
public sealed class ProcessingResult
{
    private ProcessingResult(
        ProcessingOutcome outcome,
        TimeSpan retryDelay,
        bool countsAsFailedAttempt,
        Exception? exception,
        string? reason)
    {
        Outcome = outcome;
        RetryDelay = retryDelay;
        CountsAsFailedAttempt = countsAsFailedAttempt;
        Exception = exception;
        Reason = reason;
    }

    /// <summary>
    /// The message was processed successfully.
    /// </summary>
    public static ProcessingResult Succeeded { get; } = new(ProcessingOutcome.Succeeded, TimeSpan.Zero, false, null, null);

    /// <summary>
    /// The outcome.
    /// </summary>
    public ProcessingOutcome Outcome { get; }

    /// <summary>
    /// Delay before redelivery. Only relevant for <see cref="ProcessingOutcome.Retry"/>.
    /// </summary>
    public TimeSpan RetryDelay { get; }

    /// <summary>
    /// Whether the redelivery increments the delivery attempt.
    /// <c>false</c> for deliberate delays such as rate limits and for shutdowns.
    /// </summary>
    public bool CountsAsFailedAttempt { get; }

    /// <summary>
    /// The exception that caused a retry or dead-lettering, if any.
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// Human-readable reason for dead-lettering.
    /// </summary>
    public string? Reason { get; }

    /// <summary>
    /// Requests a redelivery.
    /// </summary>
    public static ProcessingResult Retry(TimeSpan delay, Exception? exception = null, bool countsAsFailedAttempt = true)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(delay, TimeSpan.Zero);
        return new ProcessingResult(ProcessingOutcome.Retry, delay, countsAsFailedAttempt, exception, null);
    }

    /// <summary>
    /// Requests that the message is moved to the dead-letter destination.
    /// </summary>
    public static ProcessingResult DeadLetter(Exception? exception, string reason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        return new ProcessingResult(ProcessingOutcome.DeadLetter, TimeSpan.Zero, false, exception, reason);
    }
}