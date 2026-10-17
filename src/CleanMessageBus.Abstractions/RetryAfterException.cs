namespace CleanMessageBus.Abstractions;

/// <summary>
/// Thrown by a handler to request a delayed redelivery without counting it as a failed attempt,
/// e.g. when a rate-limited API asks the client to wait.
/// </summary>
public sealed class RetryAfterException : Exception
{
    /// <summary>
    /// Creates the exception.
    /// </summary>
    /// <param name="delay">Delay before the message is delivered again.</param>
    /// <param name="message">Optional message.</param>
    /// <param name="innerException">Optional inner exception.</param>
    public RetryAfterException(TimeSpan delay, string? message = null, Exception? innerException = null)
        : base(message ?? $"Processing should be retried after {delay}.", innerException)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(delay, TimeSpan.Zero);
        Delay = delay;
    }

    /// <summary>
    /// Delay before the message is delivered again.
    /// </summary>
    public TimeSpan Delay { get; }
}