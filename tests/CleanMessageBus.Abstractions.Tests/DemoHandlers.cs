using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions.Attributes;

namespace CleanMessageBus.Abstractions.Tests;

public class UnnamedDomainEventHandler: DomainEventHandlerBase<UnnamedDomainEvent>
{
    public override Task<CanFail> Handle(UnnamedDomainEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

[ForApplication("CustomApplication")]
public class ForApplicationAttributedDomainEventHandler : DomainEventHandlerBase<UnnamedDomainEvent>
{
    public override Task<CanFail> Handle(UnnamedDomainEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

[EventHandlerName("CustomDomainEventHandlerName")]
public class NamedDomainEventHandler: DomainEventHandlerBase<NamedDomainEvent>
{
    public override Task<CanFail> Handle(NamedDomainEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public class MissingForApplicationAttributeIntegrationEventHandler : IntegrationEventHandlerBase<UnnamedIntegrationEvent>
{
    public override Task<CanFail> Handle(UnnamedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

[ForApplication("CustomApplication")]
public class UnnamedIntegrationEventHandler: IntegrationEventHandlerBase<UnnamedIntegrationEvent>
{
    public override Task<CanFail> Handle(UnnamedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

[EventHandlerName("CustomIntegrationEventHandlerName")]
[ForApplication("CustomApplication")]
public class NamedIntegrationEventHandler: IntegrationEventHandlerBase<NamedIntegrationEvent>
{
    public override Task<CanFail> Handle(NamedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}