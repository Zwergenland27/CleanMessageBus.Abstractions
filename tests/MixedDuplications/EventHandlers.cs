using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;

namespace MixedDuplications;

public class EventHandlerOne: DomainEventHandlerBase<EventOne>
{
    public override Task<CanFail> Handle(EventOne domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

[EventHandlerName("EventHandlerOne")]
[ForApplication("Irrelevant")]
public class EventHandlerTwo: IntegrationEventHandlerBase<EventTwo>
{
    public override Task<CanFail> Handle(EventTwo integrationEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}