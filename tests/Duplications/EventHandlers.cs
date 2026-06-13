using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;

namespace Duplications;

public class DomainEventHandlerOne: DomainEventHandlerBase<DomainEventOne>
{
    public override Task<CanFail> Handle(DomainEventOne domainEvent, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}

[EventHandlerName("DomainEventHandlerOne")]
public class DomainEventHandlerTwo: DomainEventHandlerBase<DomainEventTwo>
{
    public override Task<CanFail> Handle(DomainEventTwo integrationEvent, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}