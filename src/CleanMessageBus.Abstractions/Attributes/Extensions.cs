namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Internal extensions for types
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Ensures that <paramref name="type"/> is either a <see cref="IIntegrationEvent"/> or <see cref="IDomainEvent"/>
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <exception cref="InvalidOperationException">Specified type is neither of the two</exception>
    internal static void EnsureValidEventType(this Type type)
    {
        if (!type.IsAssignableTo(typeof(IIntegrationEvent)) &&
            !type.IsAssignableTo(typeof(IDomainEvent)))
        {
            throw new InvalidOperationException($"Event type must be assignable to {nameof(IIntegrationEvent)} or {nameof(IDomainEvent)}");
        }
    }

    /// <summary>
    /// Ensures that <paramref name="type"/> is either a <see cref="IntegrationEventHandlerBase"/> or <see cref="DomainEventHandlerBase"/>
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <exception cref="InvalidOperationException">Specified type is neither of the two</exception>
    internal static void EnsureValidEventHandlerType(this Type type)
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
}