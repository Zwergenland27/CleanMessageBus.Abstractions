using CleanMessageBus.Abstractions.Attributes;
using InvalidDomainEventHandlers;
using InvalidIntegrationEventHandlers;

namespace CleanMessageBus.Abstractions.Tests;

public class EventHandlerNameTests
{
    [Fact]
    public void GetHandledEventName_ShouldThrow_WhenNoDomainOrIntegrationEventHandler()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => typeof(string).GetHandledEventName("application"));
        Assert.Equal("Event handler type must be assignable to IntegrationEventHandlerBase`1 or DomainEventHandlerBase`1", exception.Message);
    }
    
    [Fact]
    public void GetHandledEventName_ShouldThrow_WhenDomainEventHasSourceApplicationAttributeSet()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => typeof(InvalidDomainEventHandler).GetHandledEventName("application"));
        Assert.Equal($"Cannot set application name for InvalidDomainEventHandler since it is a domain event handler.", exception.Message);
    }

    [Theory]
    [InlineData(typeof(UnnamedDomainEventHandler), "UnnamedDomainEvent")]
    [InlineData(typeof(NamedDomainEventHandler), "CustomDomainEventName")]
    public void GetHandledEventName_ShouldReturnDomainEventName(Type handlerType, string eventName)
    {
        var domainEventName = handlerType.GetHandledEventName("application");
        Assert.Equal("application", domainEventName.ApplicationName);
        Assert.Equal(eventName, domainEventName.EventName);
    }

    [Fact]
    public void GetHandledEventName_ShouldThrow_WhenIntegrationEventHandlerMissesSourceApplicationAttribute()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => typeof(InvalidIntegrationEventHandler).GetHandledEventName("application"));
        Assert.Equal("Integration event handler InvalidIntegrationEventHandler is missing its [SourceApplication] attribute.", exception.Message);
    }

    [Theory]
    [InlineData(typeof(UnnamedIntegrationEventHandler), "CustomApplication", "UnnamedIntegrationEvent")]
    [InlineData(typeof(NamedIntegrationEventHandler), "CustomApplication", "CustomIntegrationEventName")]
    public void GetHandledEventName_ShouldReturnIntegrationEventNameWithApplicationNameFromAttribute(Type handlerType, string applicationName, string eventName)
    {
        var integrationEventName =  handlerType.GetHandledEventName("application");
        Assert.Equal(applicationName, integrationEventName.ApplicationName);
        Assert.Equal(eventName, integrationEventName.EventName);
    }
    
    
    [Fact]
    public void GetEventHandlerName_ShouldThrow_WhenNoDomainOrIntegrationEventHandler()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => typeof(string).GetEventHandlerName("application"));
        Assert.Equal("Event handler type must be assignable to IntegrationEventHandlerBase`1 or DomainEventHandlerBase`1", exception.Message);
    }

    [Theory]
    [InlineData(typeof(UnnamedDomainEventHandler), "UnnamedDomainEventHandler")]
    [InlineData(typeof(UnnamedIntegrationEventHandler), "UnnamedIntegrationEventHandler")]
    public void GetEventHandlerName_ShouldReturnApplicationAndTypeNameOnDefault(
        Type handlerType,
        string expectedHandlerName)
    {
        var handlerName =  handlerType.GetEventHandlerName("application");
        Assert.Equal("application", handlerName.ApplicationName);
        Assert.Equal(expectedHandlerName, handlerName.EventHandlerName);
    }
    
    [Theory]
    [InlineData(typeof(NamedDomainEventHandler), "CustomDomainEventHandlerName")]
    [InlineData(typeof(NamedIntegrationEventHandler), "CustomIntegrationEventHandlerName")]
    public void GetEventHandlerName_ShouldReturnApplicationAndCustomName_WhenAttributeSet(
        Type handlerType,
        string expectedHandlerName)
    {
        var handlerName =  handlerType.GetEventHandlerName("application");
        Assert.Equal("application", handlerName.ApplicationName);
        Assert.Equal(expectedHandlerName, handlerName.EventHandlerName);
    }
}