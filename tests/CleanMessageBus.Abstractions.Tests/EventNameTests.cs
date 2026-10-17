namespace CleanMessageBus.Abstractions.Tests;

public sealed class EventNamesTests
{
    [Fact]
    public void For_ShouldReturnAttributeName_WhenEventNameAttributeIsPresent()
    {
        Assert.Equal("order-shipped", EventNames.For<NamedEvent>());
    }

    [Fact]
    public void For_ShouldReturnTypeName_WhenEventNameAttributeIsMissing()
    {
        Assert.Equal(nameof(UnnamedEvent), EventNames.For(typeof(UnnamedEvent)));
    }

    [Fact]
    public void For_ShouldIgnoreAttributeOfBaseType_WhenDerivedTypeHasNone()
    {
        Assert.Equal(nameof(DerivedEvent), EventNames.For<DerivedEvent>());
    }

    [Fact]
    public void For_ShouldThrow_WhenTypeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => EventNames.For(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void EventNameAttribute_ShouldThrow_WhenNameIsEmpty(string name)
    {
        Assert.Throws<ArgumentException>(() => new EventNameAttribute(name));
    }

    [EventName("order-shipped")]
    private sealed record NamedEvent : IDomainEvent;

    private sealed record UnnamedEvent : IIntegrationEvent;

    [EventName("base-event")]
    private record BaseEvent : IDomainEvent;

    private sealed record DerivedEvent : BaseEvent;
}