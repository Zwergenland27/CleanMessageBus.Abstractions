namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Defines that the handler should only preload a specific amount of events from the broker 
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SequentialAttribute : Attribute
{
    /// <summary>
    /// Defines the amount of events that can be retrieved from the broker in the local queue
    /// </summary>
    public required int Amount { get; init; } = 1;
}