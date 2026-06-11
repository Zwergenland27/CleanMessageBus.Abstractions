using System.Reflection;

namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Extension method to get the consumer name of a type
/// </summary>
public static class ConsumedByAttributeExtensions
{
    /// <summary>
    /// Extracts the name of the consumer for an event handler
    /// </summary>
    /// <param name="eventHandlerType">Type of the event handler</param>
    /// <returns>Name of the consumer, either custom or automatically generated</returns>
    public static string GetConsumerName(this Type eventHandlerType)
    {
        eventHandlerType.EnsureValidEventHandlerType();
        
        var consumedByAttribute = eventHandlerType
            .GetCustomAttribute<ConsumedByAttribute>(false);

        if (consumedByAttribute is not null)
        {
            return consumedByAttribute.Name;
        }
        
        var namespaceName = eventHandlerType.Namespace!;
        var typeName = eventHandlerType.Name;
        
        return $"{namespaceName}:{typeName}";
    }
}