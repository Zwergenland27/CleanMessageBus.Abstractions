# CleanMessageBus
This package provides an abstraction layer for message busses for easy integration of events.

## Publishing Events
* Event object must inherit from class ``Integrationevent`` for integration events ot ``DomainEvent`` for domain events
* Inject ``IMessageBus`` to desired service
* Publish event by using ``IMessageBus.PublishAsync(event)``
* Automatic creation of producer resource on message bus

* Example:
```csharp
public class UserRegisteredEvent(string Username, string Email) : Integrationevent;

var integrationEvent = new UserRegisteredEvent("JohnDoe", "john@doe.com");
await messageBus.PublishAsync(integrationevent);
```

### Change event name name
* Use attribute ``[EventName(<Name>)]`` to set producer name

## Handling Events
* Handler must inherit from ``IntegrationEventHandlerBase<TIntegrationEvent>`` class for integration events or from ``DomainEventHandlerBase<TDomainEvent>`` for domain events
* Integration event handlers must specify the name of the application, that published the event using the `[SourceApplication(<Name>)]` attribute. This is **not allowed** for domain event handlers, as the application of the event and the event handler must match.
* Generic type specifies domain event type to receive
* Implementation logic must be implemented in the ``Handle`` method
* Registration happens automatically via dependency injection
* Integration of [CleanDomainValidation](https://github.com/Zwergenland27/CleanDomainValidation) result types

Example:

```csharp
[SourceApplication("MyApp.Authorization")]
public class UserRegisteredEventHandler : IntegrationEventHandlerBase<UserRegisteredEvent>
{
    public override async Task<CanFail> Handle(UserRegisteredEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"User {@event.Username} with email {@event.Email} has been registered.");
        
        //TODO furhter logic in here
        
        return CanFail.Success;
    }
}
```

### Change event handler name
* Use attribute ``[EventHandlerName(<Name>)]`` to set event handler name

### Use serialized message handling
* Ideal for long running / rate limited tasks, that should be split across multiple instances
* Use attribute ``[Serialized(MaxQueueSize = 1)]`` to set the max amount of messages that is being loaded in the application simultaniously
* Event handlers with this attribute are only called if the previous handler has completed processing the event
* Rate limiting may be implemented in the handler itself

## Configuration via dependency injection
* Define assemblies containing events and handlers
* Define name of the current application (defaults to name of the entry assembly)
* Select and configure message broker

Example:
```csharp
builder.Services.AddCleanMessageBus(config => config
    .WithApplicationName("MyApp")
    .RegisterIntegrationEventsFromAssembly(Assembly.GetExecutingAssembly())
    .RegisterDomainEventsFromAssemby(Assembly.GetExecutingAssembly())
    .RegisterHandlersFromAssemby(Assembly.GetExecutingAssembly())
    .Use... //Use concrete implementation like rabbbitmq
```