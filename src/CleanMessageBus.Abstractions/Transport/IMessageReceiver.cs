namespace CleanMessageBus.Abstractions.Transport;

/// <summary>
/// Entry point for transports to hand received messages to the consume pipeline.
/// </summary>
public interface IMessageReceiver
{
    /// <summary>
    /// Processes a message for a handler. Never throws for handler failures; the returned
    /// <see cref="ProcessingResult"/> tells the transport whether to acknowledge, retry or dead-letter.
    /// </summary>
    ValueTask<ProcessingResult> ReceiveAsync(
        MessageEnvelope envelope,
        EventHandlerDescriptor handler,
        DeliveryInfo delivery,
        CancellationToken cancellationToken);
}