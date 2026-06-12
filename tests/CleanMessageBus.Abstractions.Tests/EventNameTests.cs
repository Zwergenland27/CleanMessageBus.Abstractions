using CleanMessageBus.Abstractions.Attributes;

namespace CleanMessageBus.Abstractions.Tests;

public class EventNameTests
{
    [Fact]
    public void GetEventName_ShouldThrow_WhenNoDomainOrIntegrationEvent()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => typeof(string).GetEventName("application"));
        Assert.Equal("Event type must be assignable to IntegrationEvent or DomainEvent", exception.Message);
    }
    [Theory]
    [InlineData(typeof(UnnamedDomainEvent), "UnnamedDomainEvent")]
    [InlineData(typeof(UnnamedIntegrationEvent), "UnnamedIntegrationEvent")]
    public void GetEventName_ShouldReturnApplicationAndTypeName_OnDefault(Type eventType, string expectedName)
    {
        var name = eventType.GetEventName("application");
        
        Assert.Equal("application", name.ApplicationName);
        Assert.Equal(expectedName, name.EventName);
    }
    
    [Theory]
    [InlineData(typeof(NamedDomainEvent), "CustomDomainEventName")]
    [InlineData(typeof(NamedIntegrationEvent), "CustomIntegrationEventName")]
    public void GetEventName_ShouldReturnApplicationAndCustomName_WhenAttributeSet(Type eventType, string expectedName)
    {
        var name = eventType.GetEventName("application");
        
        Assert.Equal("application", name.ApplicationName);
        Assert.Equal(expectedName, name.EventName);
    }
}