namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Properties for serialized handlers
/// </summary>
public class SerializationProperties
{
    /// <summary>
    /// The maximum number of events that can be loaded in the application simultaneously
    /// </summary>
    public uint MaxQueueSize { get; init; }
}