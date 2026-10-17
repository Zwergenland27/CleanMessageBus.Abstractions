namespace CleanMessageBus.Abstractions;

/// <summary>
/// The payload of a message could not be deserialized. Such messages are dead-lettered without retries.
/// </summary>
public sealed class MessageDeserializationException: Exception
{
    /// <summary>
    /// Creates the exception.
    /// </summary>
    public MessageDeserializationException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}