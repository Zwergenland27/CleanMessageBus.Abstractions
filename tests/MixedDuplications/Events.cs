using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;

namespace MixedDuplications;

public class EventOne: DomainEvent;

[EventName("EventOne")]
public class EventTwo: IntegrationEvent;