using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions.Attributes;

namespace CleanMessageBus.Abstractions.Tests;

public class UnnamedDomainEventHandler: DomainEventHandlerBase<UnnamedDomainEvent>
{
    public override Task<CanFail> Handle(UnnamedDomainEvent @event, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}

[EventHandlerName("CustomDomainEventHandlerName")]
public class NamedDomainEventHandler: DomainEventHandlerBase<NamedDomainEvent>
{
    public override Task<CanFail> Handle(NamedDomainEvent @event, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}

[ForApplication("CustomApplication")]
public class UnnamedIntegrationEventHandler: IntegrationEventHandlerBase<UnnamedIntegrationEvent>
{
    public override Task<CanFail> Handle(UnnamedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}

[EventHandlerName("CustomIntegrationEventHandlerName")]
[ForApplication("CustomApplication")]
public class NamedIntegrationEventHandler: IntegrationEventHandlerBase<NamedIntegrationEvent>
{
    public override Task<CanFail> Handle(NamedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}