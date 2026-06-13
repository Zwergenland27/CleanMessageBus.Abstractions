using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;

namespace InvalidDomainEventHandlers;

public class InvalidDomainEvent: DomainEvent;

[SourceApplication("ApplicationName")]
public class InvalidDomainEventHandler: DomainEventHandlerBase<InvalidDomainEvent>
{
    public override Task<CanFail> Handle(InvalidDomainEvent invalidDomainEvent, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}