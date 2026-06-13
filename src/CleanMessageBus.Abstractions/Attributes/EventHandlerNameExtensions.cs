using System.Reflection;

namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Extension methods to get the event handler name
/// </summary>
public static class EventHandlerNameExtensions
{
    private static void EnsureValidHandlerType(this Type type)
    {
        var integrationEventHandlerType = typeof(IntegrationEventHandlerBase<>);
        var domainEventHandlerType = typeof(DomainEventHandlerBase<>);
        
        if (type.BaseType is not { IsGenericType: true })
        {
            throw new InvalidOperationException($"Event handler type must be assignable to {integrationEventHandlerType.Name} or {domainEventHandlerType.Name}");
        }
        
        if(type.BaseType.GetGenericTypeDefinition() != integrationEventHandlerType &&
           type.BaseType.GetGenericTypeDefinition() != domainEventHandlerType)
        {
            throw new InvalidOperationException($"Event handler type must be assignable to {integrationEventHandlerType.Name} or {domainEventHandlerType.Name}");
        }
    }

    private static bool IsDomainEventHandler(this Type type)
    {
        var integrationEventHandlerType = typeof(IntegrationEventHandlerBase<>);
        var domainEventHandlerType = typeof(DomainEventHandlerBase<>);
        
        if (type.BaseType is not { IsGenericType: true })
        {
            throw new InvalidOperationException($"Event handler type must be assignable to {integrationEventHandlerType.Name} or {domainEventHandlerType.Name}");
        }
        
        return type.BaseType.GetGenericTypeDefinition() == domainEventHandlerType;
    }

    /// <summary>
    /// Checks if the handler is annotated with the <see cref="SourceApplicationAttribute"/>
    /// </summary>
    /// <param name="handlerType">Type of the event handler</param>
    /// <returns>True, if the attribute is used on the class</returns>
    public static bool IsAnnotatedWithSourceApplicationAttribute(this Type handlerType)
    {
        var eventHandlerNameAttribute = handlerType
            .GetCustomAttribute<SourceApplicationAttribute>(false);

        return eventHandlerNameAttribute is not null;
    }
    
    /// <summary>
    /// Retrieves the name of the event that is handled by the event handler
    /// </summary>
    /// <param name="handlerType">Type of the event handler</param>
    /// /// <param name="applicationName">Name of the current application</param>
    /// <exception cref="InvalidOperationException"><see cref="SourceApplicationAttribute"/> has not been set for the handler</exception>
    public static UniqueEventName GetHandledEventName(this Type handlerType, string applicationName)
    {
        handlerType.EnsureValidHandlerType();

        if (handlerType.IsDomainEventHandler())
        {
            return handlerType.GetHandledDomainEventName(applicationName);
        }

        return handlerType.GetHandledIntegrationEventName();
    }
    
    /// <summary>
    /// Retrieves the name of the event handler
    /// </summary>
    /// <param name="handlerType">Type of the event handler</param>
    /// /// <param name="applicationName">Name of the current application</param>
    /// <exception cref="InvalidOperationException"><see cref="SourceApplicationAttribute"/> has not been set for the handler</exception>
    public static UniqueEventHandlerName GetEventHandlerName(this Type handlerType, string applicationName)
    {
        handlerType.EnsureValidHandlerType();
        
        var eventHandlerNameAttribute = handlerType
            .GetCustomAttribute<EventHandlerNameAttribute>(false);
        
        var eventHandlerName = handlerType.Name;
        if (eventHandlerNameAttribute is not null)
        {
            eventHandlerName = eventHandlerNameAttribute.Name;
        }

        return new UniqueEventHandlerName(applicationName, eventHandlerName);
    }

    private static UniqueEventName GetHandledDomainEventName(this Type handlerType, string applicationName)
    {
        var sourceApplicationAttribute = handlerType
            .GetCustomAttribute<SourceApplicationAttribute>(false);
        if (sourceApplicationAttribute is not null)
        {
            throw new InvalidOperationException($"Cannot set application name for {handlerType.Name} since it is a domain event handler.");
        }
        
        //Domain event that is handled by the handler
        var handledDomainEventType = handlerType.BaseType!.GetGenericArguments()[0];

        return handledDomainEventType.GetEventName(applicationName);
    }

    private static UniqueEventName GetHandledIntegrationEventName(this Type handlerType)
    {
        var sourceApplicationAttribute = handlerType
            .GetCustomAttribute<SourceApplicationAttribute>(false);
        
        if (sourceApplicationAttribute is null)
        {
            throw new InvalidOperationException(
                $"Integration event handler {handlerType.Name} is missing its [SourceApplication] attribute.");
        }
        
        //Integration event that is handled by the handler
        var integrationEventType = handlerType.BaseType!.GetGenericArguments()[0];
        
        return integrationEventType.GetEventName(sourceApplicationAttribute.ApplicationName);
    }
}