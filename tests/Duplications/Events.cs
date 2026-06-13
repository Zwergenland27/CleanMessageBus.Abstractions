using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;

namespace Duplications;

public class DomainEventOne: DomainEvent;

[EventName("DomainEventOne")]
public class DomainEventTwo:  DomainEvent;

[EventName("IntegrationEventOne")]
public class IntegrationEventOne: IntegrationEvent;

[EventName("IntegrationEventOne")]
public class IntegrationEventTwo: IntegrationEvent;