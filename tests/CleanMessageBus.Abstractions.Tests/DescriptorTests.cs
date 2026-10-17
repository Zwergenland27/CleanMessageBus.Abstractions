namespace CleanMessageBus.Abstractions.Tests;

public sealed class DescriptorTests
{
    [Fact]
    public void EventHandlerDescriptor_ShouldCopyProperties_WhenSourceChangesAfterwards()
    {
        var properties = new Dictionary<string, object?> { ["key"] = 1 };

        var descriptor = CreateHandler(properties: properties);
        properties["other"] = 2;

        Assert.Single(descriptor.Properties);
        Assert.Equal(1, descriptor.Properties["key"]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(EventHandlerDescriptor.MaxPrefetchCount + 1)]
    public void EventHandlerDescriptor_ShouldThrow_WhenPrefetchCountIsOutOfRange(int prefetchCount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateHandler(prefetchCount: prefetchCount));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void EventHandlerDescriptor_ShouldThrow_WhenNameIsEmpty(string name)
    {
        Assert.Throws<ArgumentException>(() => CreateHandler(name: name));
    }

    [Fact]
    public void DeliveryInfo_ShouldThrow_WhenAttemptIsLessThanOne()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DeliveryInfo(0));
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public void DeliveryInfo_ShouldDetectRedelivery_WhenAttemptIsGreaterThanOne(int attempt, bool expected)
    {
        Assert.Equal(expected, new DeliveryInfo(attempt).IsRedelivery);
    }

    [Fact]
    public void MessageEnvelope_ShouldHaveEmptyHeaders_WhenNoHeadersAreGiven()
    {
        var envelope = new MessageEnvelope
        {
            MessageId = Guid.NewGuid(),
            EventName = "OrderPlaced",
            Kind = EventKind.Domain,
            SourceApplication = "app",
            Body = Array.Empty<byte>(),
            ContentType = "application/json",
            Timestamp = DateTimeOffset.UtcNow,
        };

        Assert.Empty(envelope.Headers);
    }

    private static EventHandlerDescriptor CreateHandler(
        string name = "handler",
        int prefetchCount = 1,
        IReadOnlyDictionary<string, object?>? properties = null) =>
        new(name, typeof(object), "Event", EventKind.Domain, "app", prefetchCount, RetryPolicy.None, properties);
}