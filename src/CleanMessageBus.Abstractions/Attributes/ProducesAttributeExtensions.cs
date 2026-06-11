using System.Reflection;

namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Extension method to get the producer name of a type
/// </summary>
public static class ProducesAttributeExtensions
{
    /// <summary>
    /// Extracts the producer name from event
    /// </summary>
    /// <param name="eventType">Type of the event</param>
    /// <returns>Name of the producer, either custom or automatically generated</returns>
    public static string GetProducerName(this Type eventType)
    {
        eventType.EnsureValidEventType();
        
        var producesAttribute = eventType
            .GetCustomAttribute<ProducesAttribute>(false);

        if (producesAttribute is not null)
        {
            return producesAttribute.Name;
        }
       
        var namespaceName = eventType.Namespace!;
        var typeName = eventType.Name;
        
        return $"{namespaceName}:{typeName}";
    }
}