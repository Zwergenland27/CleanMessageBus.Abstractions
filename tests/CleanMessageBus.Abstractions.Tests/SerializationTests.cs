using CleanMessageBus.Abstractions.Attributes;

namespace CleanMessageBus.Abstractions.Tests;

public class SerializationTests
{
    [Fact]
    public void GetSerializationInformation_ShouldThrow_WhenNoDomainOrIntegrationEventHandler()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => typeof(string).GetSerializationInformation());
        Assert.Equal("Event handler type must be assignable to IntegrationEventHandlerBase`1 or DomainEventHandlerBase`1", exception.Message);
    }

    [Fact]
    public void GetSerializationInformation_ShouldReturnNull_WhenNoSerializationAttributeSet()
    {
        var serializationProperties = typeof(UnnamedDomainEventHandler).GetSerializationInformation();
        Assert.Null(serializationProperties);
    }
    
    [Fact]
    public void GetSerializationInformation_ShouldReturnSerializationProperties_WhenNoSerializationAttributeSet()
    {
        var serializationProperties = typeof(SerializedDomainEventHandler).GetSerializationInformation();
        Assert.NotNull(serializationProperties);
        Assert.Equal((uint) 42, serializationProperties.MaxQueueSize);
    }
}