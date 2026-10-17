# CleanMessageBus.Abstractions

Dependency-free contracts of CleanMessageBus. Reference this package from your domain layer to get the
event marker interfaces without pulling in any infrastructure.

| Area | Types |
| --- | --- |
| Events | `IDomainEvent`, `IIntegrationEvent`, `EventNameAttribute`, `EventNames`, `EventKind` |
| Messages | `MessageEnvelope`, `MessageHeaderNames`, `MessageTraceContext`, `EventContext`, `DeliveryInfo` |
| Handling | `EventHandlerDescriptor`, `RetryPolicy`, `ProcessingResult`, `RetryAfterException`, `MessageDeserializationException` |
| Publishing | `IEventPublisher`, `PublishOptions`, `IMessageSerializer` |
| Transport (`CleanMessageBus.Transport`) | `IBrokerTransport`, `IMessageReceiver`, `TransportStartContext` |
| Telemetry | `CleanMessageBusDiagnostics` (activity source and meter names) |

```csharp
[EventName("trip-booked")] // optional: keeps the routing name stable when the type is renamed
public sealed record TripBooked(Guid TripId) : IDomainEvent;

public sealed record TripPayed(Guid TripId) : IIntegrationEvent;
```

## Build & test

```bash
dotnet test
dotnet pack -c Release -o ./artifacts
```
