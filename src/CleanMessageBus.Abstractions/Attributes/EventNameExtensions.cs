using System.Reflection;

namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Extension methods to get the event name
/// </summary>
public static class EventNameExtensions
{
    private static void EnsureValidEventType(this Type type)
    {
        if (!type.IsAssignableTo(typeof(IntegrationEvent)) &&
            !type.IsAssignableTo(typeof(DomainEvent)))
        {
            throw new InvalidOperationException($"Event type must be assignable to {nameof(IntegrationEvent)} or {nameof(DomainEvent)}");
        }
    }
    
    /// <summary>
    /// Retrieves the full name of an event
    /// </summary>
    /// <param name="eventType">Event type</param>
    /// <param name="applicationName">Name of the application the event occured in</param>
    public static UniqueEventName GetEventName(this Type eventType, string applicationName)
    {
        eventType.EnsureValidEventType();
        
        var eventNameAttribute = eventType
            .GetCustomAttribute<EventNameAttribute>(false);

        var eventName = eventType.Name;

        if (eventNameAttribute is not null)
        {
            eventName = eventNameAttribute.Name;
        }

        return new UniqueEventName(applicationName, eventName);
    }
}