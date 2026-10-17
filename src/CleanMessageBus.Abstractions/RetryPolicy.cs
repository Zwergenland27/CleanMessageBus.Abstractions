namespace CleanMessageBus.Abstractions;

/// <summary>
/// Describes how often and with which delays a failed message is retried before it is dead-lettered.
/// </summary>
public sealed class RetryPolicy
{
    /// <summary>
    /// reates a policy with one retry per interval.
    /// </summary>
    /// <param name="intervals">Delay before each retry. The number of entries is the number of retries.</param>
    public RetryPolicy(IEnumerable<TimeSpan> intervals)
    {
        ArgumentNullException.ThrowIfNull(intervals);

        var copy = intervals.ToArray();
        foreach (var interval in copy)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(interval, TimeSpan.Zero, nameof(intervals));
        }

        Intervals = copy;
    }

    /// <summary>
    /// No retries: failed messages are dead-lettered immediately.
    /// </summary>
    public static RetryPolicy None { get; } = new([]);

    /// <summary>
    /// Three retries after 5 seconds, 30 seconds and 2 minutes.
    /// </summary>
    public static RetryPolicy Default { get; } =
        new([TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(2)]);

    /// <summary>
    /// Delay before each retry.
    /// </summary>
    public IReadOnlyList<TimeSpan> Intervals { get; }

    /// <summary>
    /// Maximum number of retries.
    /// </summary>
    public int MaxRetries => Intervals.Count;

    /// <summary>
    /// Creates a policy with <paramref name="retries"/> retries using the same interval.
    /// </summary>
    public static RetryPolicy Fixed(int retries, TimeSpan interval)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(retries);
        return new RetryPolicy(Enumerable.Repeat(interval, retries));
    }

    /// <summary>
    /// Creates a policy whose interval is multiplied by <paramref name="factor"/> after each retry.
    /// </summary>
    public static RetryPolicy Exponential(int retries, TimeSpan initialInterval, double factor = 2)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(retries);
        ArgumentOutOfRangeException.ThrowIfLessThan(factor, 1d);

        var intervals = new TimeSpan[retries];
        var current = initialInterval;
        for (var i = 0; i < retries; i++)
        {
            intervals[i] = current;
            current = TimeSpan.FromTicks((long)(current.Ticks * factor));
        }

        return new RetryPolicy(intervals);
    }

    /// <summary>
    /// Returns the delay before the next retry after the given attempt failed.
    /// </summary>
    /// <param name="failedAttempt">The 1-based attempt that failed.</param>
    /// <param name="delay">The delay before the retry.</param>
    /// <returns><c>false</c> if no retries are left.</returns>
    public bool TryGetRetryDelay(int failedAttempt, out TimeSpan delay)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(failedAttempt, 1);

        if (failedAttempt <= Intervals.Count)
        {
            delay = Intervals[failedAttempt - 1];
            return true;
        }

        delay = TimeSpan.Zero;
        return false;
    }
}