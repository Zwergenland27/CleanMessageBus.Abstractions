using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;

namespace MixedDuplications;

public class EventHandlerOne: DomainEventHandlerBase<EventOne>
{
    public override Task<CanFail> Handle(EventOne domainEvent, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}

[EventHandlerName("EventHandlerOne")]
[SourceApplication("Irrelevant")]
public class EventHandlerTwo: IntegrationEventHandlerBase<EventTwo>
{
    public override Task<CanFail> Handle(EventTwo integrationEvent, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}