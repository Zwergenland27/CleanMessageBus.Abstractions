using System.Reflection;

namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Extension method to get information about sequential execution
/// </summary>
public static class SequentialAttributeExtensions
{
    /// <summary>
    /// Extracts the queue size from an event handler
    /// </summary>
    /// <param name="eventHandlerType">Type of the event handler</param>
    /// <returns>Throttled request interval in milliseconds if set, or null</returns>
    public static uint GetQueueSize(this Type eventHandlerType)
    {
        eventHandlerType.EnsureValidEventHandlerType();
        
        var sequentialAttribute = eventHandlerType
            .GetCustomAttribute<SequentialAttribute>(false);

        if (sequentialAttribute is null)
        {
            return uint.MaxValue;
        }
        
        return sequentialAttribute.QueueSize;
    }
}