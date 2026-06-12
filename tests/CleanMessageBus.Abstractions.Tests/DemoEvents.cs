using CleanMessageBus.Abstractions.Attributes;

namespace CleanMessageBus.Abstractions.Tests;

public class UnnamedDomainEvent: DomainEvent;

[EventName("CustomDomainEventName")]
public class NamedDomainEvent: DomainEvent;

public class UnnamedIntegrationEvent: IntegrationEvent;

[EventName("CustomIntegrationEventName")]
public class NamedIntegrationEvent: IntegrationEvent;