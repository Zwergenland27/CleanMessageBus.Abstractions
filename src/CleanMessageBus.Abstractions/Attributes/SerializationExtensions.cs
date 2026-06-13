using System.Reflection;

namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Extension methods to get information about the serialization properties of an event handler
/// </summary>
public static class SerializationExtensions
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

    /// <summary>
    /// Extract serialization information for an event handler
    /// </summary>
    /// <param name="handlerType">Type of the event handler to get the properties from</param>
    /// <returns>Null, when no serialization has been set up; The specified properties otherwise</returns>
    public static SerializationProperties? GetSerializationInformation(this Type handlerType)
    {
        handlerType.EnsureValidHandlerType();
        
        var serialized = handlerType
            .GetCustomAttribute<SerializedAttribute>(false);

        if (serialized is null)
        {
            return null;
        }

        return new SerializationProperties
        {
            MaxQueueSize = serialized.MaxQueueSize,
        };
    }
}