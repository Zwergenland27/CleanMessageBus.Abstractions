namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Marks handlers that should serialize incoming requests
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SerializedAttribute: Attribute
{
    /// <summary>
    /// The maximum number of events that can be loaded in the application simultaneously
    /// </summary>
    public required uint MaxQueueSize { get; init; }
}