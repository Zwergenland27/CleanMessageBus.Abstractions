using System.Reflection;

namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Extension method to get the produced name of a type
/// </summary>
public static class ProducedByAttributeExtensions
{
    /// <summary>
    /// Extracts the name of the producing event for an event handler
    /// </summary>
    /// <param name="eventHandlerType">Type of the event handler</param>
    /// <returns>Name of the producer, either custom or automatically generated</returns>
    public static string GetProducedByName(this Type eventHandlerType)
    {
        eventHandlerType.EnsureValidEventHandlerType();
        
        var producedByAttribute = eventHandlerType
            .GetCustomAttribute<ProducedByAttribute>(false);

        if (producedByAttribute is not null && 
            eventHandlerType.BaseType!.GetGenericTypeDefinition() == typeof(DomainEventHandlerBase<>))
        {
            throw new InvalidOperationException("ProducedBy attribute is not allowed to be set for domain event handlers. The correct producer name will be automatically generated from the domain event type.");
        }
        
        if (producedByAttribute is not null)
        {
            return producedByAttribute.Name;
        }
        
        var eventType = eventHandlerType.BaseType!.GetGenericArguments()[0];

        return eventType.GetProducerName();
    }
}