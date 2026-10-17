namespace CleanMessageBus.Abstractions;

/// <summary>
/// What a transport should do with a message after it has been processed.
/// </summary>
public enum ProcessingOutcome
{
    /// <summary>
    /// The message was processed and can be acknowledged.
    /// </summary>
    Succeeded,

    /// <summary>
    /// The message should be delivered again using the specified retry policy.
    /// </summary>
    Retry,

    /// <summary>
    /// The message must not be delivered again and should be moved to the dead-letter destination.
    /// </summary>
    DeadLetter,
}