using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;

namespace InvalidIntegrationEventHandlers;

public class InvalidIntegrationEvent: IntegrationEvent;

public class InvalidIntegrationEventHandler: IntegrationEventHandlerBase<InvalidIntegrationEvent>
{
    public override Task<CanFail> Handle(InvalidIntegrationEvent invalidIntegrationEvent, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}