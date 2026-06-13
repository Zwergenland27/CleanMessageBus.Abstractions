using System.Reflection;
using CleanMessageBus.Abstractions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMessageBus.Abstractions;

/// <summary>
/// Configuration for the CleanMessageBus
/// </summary>
public class CleanMessageBusConfiguration(IServiceCollection services)
{
    /// <summary>
    /// ServiceCollection for internal usage passed in from dependency injection
    /// </summary>
    public readonly IServiceCollection Services = services;
    
    private readonly HashSet<Type> _integrationEvents = [];
    private readonly HashSet<Type> _integrationEventHandlers = [];
    private readonly HashSet<Type> _domainEvents = [];
    private readonly HashSet<Type> _domainEventHandlers = [];

    private readonly HashSet<string> _eventNames = [];
    private readonly HashSet<string> _eventHandlerNames = [];
    
    /// <summary>
    /// List of registered integration event types
    /// </summary>
    public IReadOnlyCollection<Type> IntegrationEvents => _integrationEvents.ToList().AsReadOnly();
    
    /// <summary>
    /// List of registered integration event handler types
    /// </summary>
    public IReadOnlyCollection<Type> IntegrationEventHandlers => _integrationEventHandlers.ToList().AsReadOnly();
    
    /// <summary>
    /// List of registered domain event types
    /// </summary>
    public IReadOnlyCollection<Type> DomainEvents => _domainEvents.ToList().AsReadOnly();
    
    /// <summary>
    /// List of registered domain event handler types
    /// </summary>
    public IReadOnlyCollection<Type> DomainEventHandlers => _domainEventHandlers.ToList().AsReadOnly();

    /// <summary>
    /// Indicates whether one message bus is registered
    /// </summary>
    public bool MessageBusRegistered = false;
    
    /// <summary>
    /// Name of the current application
    /// </summary>
    public string ApplicationName = Assembly.GetEntryAssembly()!.GetName().Name!;

    /// <summary>
    /// Register handlers from assembly <paramref name="assembly"/>
    /// </summary>
    public CleanMessageBusConfiguration RegisterHandlersFromAssembly(Assembly assembly)
    {
        RegisterIntegrationEventHandlers(assembly);
        RegisterDomainEventHandlers(assembly);
        return this;
    }

    /// <summary>
    /// Register handlers from list of <paramref name="assemblies"/>
    /// </summary>
    public CleanMessageBusConfiguration RegisterHandlersFromAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            RegisterIntegrationEventHandlers(assembly);
            RegisterDomainEventHandlers(assembly);
        }
        return this;
    }
    
    /// <summary>
    /// Register integration events from assembly <paramref name="assembly"/>
    /// </summary>
    public CleanMessageBusConfiguration RegisterIntegrationEventsFromAssembly(Assembly assembly)
    {
        RegisterIntegrationEvents(assembly);
        return this;
    }

    /// <summary>
    /// Register integration events from list of <paramref name="assemblies"/>
    /// </summary>
    public CleanMessageBusConfiguration RegisterIntegrationEventsFromAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            RegisterIntegrationEvents(assembly);
        }
        
        return this;
    }
    
    /// <summary>
    /// Register domain events from assembly <paramref name="assembly"/>
    /// </summary>
    public CleanMessageBusConfiguration RegisterDomainEventsFromAssembly(Assembly assembly)
    {
        RegisterDomainEvents(assembly);
        return this;
    }

    /// <summary>
    /// Register domain events from list of <paramref name="assemblies"/>
    /// </summary>
    public CleanMessageBusConfiguration RegisterDomainEventsFromAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            RegisterDomainEvents(assembly);
        }
        
        return this;
    }

    /// <summary>
    /// Sets the application name used as producer and consumer prefix
    /// </summary>
    /// <param name="applicationName">Name of this application</param>
    public CleanMessageBusConfiguration WithApplicationName(string applicationName)
    {
        ApplicationName = applicationName;
        return this;
    }
    
    private void RegisterIntegrationEventHandlers(Assembly assembly)
    {
        Type handlerType = typeof(IntegrationEventHandlerBase<>);
        var handlerTypes = assembly.GetTypes()
            .Where(type =>
                type.BaseType is { IsGenericType: true } && 
                type.BaseType.GetGenericTypeDefinition() == handlerType);
        
        foreach (var handler in handlerTypes)
        {
            var eventHandlerName = handler.GetEventHandlerName(ApplicationName).EventHandlerName;
            
            var hasForApplicationAttribute = handler.IsAnnotatedWithForApplicationAttribute();
            if (!hasForApplicationAttribute)
            {
                throw new InvalidOperationException(
                    $"Integration event handler {eventHandlerName} is missing its [ForApplication] attribute.");
            }
            
            if (_eventHandlerNames.Contains(eventHandlerName))
            {
                throw new InvalidOperationException($"Duplicate event handler with name {eventHandlerName}");
            }
            
            Services.AddTransient(handler);
            _integrationEventHandlers.Add(handler);
            _eventHandlerNames.Add(eventHandlerName);
        }
    }
    
    private void RegisterDomainEventHandlers(Assembly assembly)
    {
        Type handlerType = typeof(DomainEventHandlerBase<>);
        var handlerTypes = assembly.GetTypes()
            .Where(type =>
                type.BaseType is { IsGenericType: true } && 
                type.BaseType.GetGenericTypeDefinition() == handlerType);
        
        foreach (var handler in handlerTypes)
        {
            var eventHandlerName = handler.GetEventHandlerName(ApplicationName).EventHandlerName;
            
            var hasForApplicationAttribute = handler.IsAnnotatedWithForApplicationAttribute();
            if (hasForApplicationAttribute)
            {
                throw new InvalidOperationException($"Cannot set application name for {eventHandlerName} since it is a domain event handler.");
            }
            
            if (_eventHandlerNames.Contains(eventHandlerName))
            {
                throw new InvalidOperationException($"Duplicate event handler with name {eventHandlerName}");
            }
            Services.AddTransient(handler);
            _domainEventHandlers.Add(handler);
            _eventHandlerNames.Add(eventHandlerName);
        }
    }

    private void RegisterIntegrationEvents(Assembly assembly)
    {
        var integrationEvents = assembly
            .GetTypes()
            .Where(p => p.IsAssignableTo(typeof(IntegrationEvent)));

        foreach (var integrationEvent in integrationEvents)
        {
            var eventName = integrationEvent.GetEventName(ApplicationName).EventName;
            if (_eventNames.Contains(eventName))
            {
                throw new InvalidOperationException($"Duplicate event with name {eventName}");
            }
            _integrationEvents.Add(integrationEvent);
            _eventNames.Add(eventName);
        }
    }

    private void RegisterDomainEvents(Assembly assembly)
    {
        var domainEvents = assembly
            .GetTypes()
            .Where(p => p.IsAssignableTo(typeof(DomainEvent)));

        foreach (var domainEvent in domainEvents)
        {
            var eventName = domainEvent.GetEventName(ApplicationName).EventName;
            if (_eventNames.Contains(eventName))
            {
                throw new InvalidOperationException($"Duplicate event with name {eventName}");
            }
            _domainEvents.Add(domainEvent);
            _eventNames.Add(eventName);
        }
    }
}