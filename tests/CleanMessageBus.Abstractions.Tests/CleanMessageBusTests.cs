using System.Reflection;
using Duplications;
using InvalidDomainEventHandlers;
using InvalidIntegrationEventHandlers;
using Microsoft.Extensions.DependencyInjection;
using MixedDuplications;
using Xunit.Sdk;

namespace CleanMessageBus.Abstractions.Tests;

public class CleanMessageBusTests
{
    private static CleanMessageBusConfiguration CreateSut()
    {
        var services = new ServiceCollection();
        return new CleanMessageBusConfiguration(services);
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyCollections()
    {
        var sut = CreateSut();
        
        Assert.Empty(sut.IntegrationEvents);
        Assert.Empty(sut.DomainEvents);
        
        Assert.Empty(sut.DomainEventHandlers);
        Assert.Empty(sut.IntegrationEventHandlers);
    }
    
    [Fact]
    public void Constructor_ShouldSetMessageBusRegisteredToFalse_WhenCreated()
    {
        var sut = CreateSut();

        Assert.False(sut.MessageBusRegistered);
    }
    
    [Fact]
    public void Constructor_ShouldSetApplicationNameToEntryAssemblyName_WhenCreated()
    {
        var sut = CreateSut();

        //Cannot be tested since entry assembly name differs with test suite and IDE
        Assert.False(string.IsNullOrWhiteSpace(sut.ApplicationName));
    }
    
    [Fact]
    public void WithApplicationName_ShouldSetApplicationName_WhenCalled()
    {
        var sut = CreateSut();

        var result = sut.WithApplicationName("ApplicationName");

        Assert.Equal("ApplicationName", sut.ApplicationName);
        Assert.Same(sut, result);
    }
    
    [Fact]
    public void RegisterIntegrationEventsFromAssembly_ShouldAddIntegrationEvent_WhenAssemblyContainsIntegrationEvent()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetExecutingAssembly();

        var result = sut.RegisterIntegrationEventsFromAssembly(assembly);

        Assert.Contains(typeof(UnnamedIntegrationEvent), sut.IntegrationEvents);
        Assert.Same(sut, result);
    }
    
    [Fact]
    public void RegisterIntegrationEventsFromAssembly_ShouldNotAddDomainEvent_WhenAssemblyContainsDomainEvent()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetExecutingAssembly();

        sut.RegisterIntegrationEventsFromAssembly(assembly);

        Assert.DoesNotContain(typeof(UnnamedDomainEvent), sut.IntegrationEvents);
    }
    
    [Fact]
    public void RegisterIntegrationEventsFromAssembly_ShouldThrowException_WhenDuplicateEventNameFound()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetAssembly(typeof(DomainEventOne))!;

        var exception = Assert.Throws<InvalidOperationException>(() => sut.RegisterIntegrationEventsFromAssembly(assembly));
        
        Assert.Equal("Duplicate event with name IntegrationEventOne", exception.Message);
    }
    
    [Fact]
    public void RegisterDomainEventsFromAssembly_ShouldAddDomainEvent_WhenAssemblyContainsDomainEvent()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetExecutingAssembly();

        var result = sut.RegisterDomainEventsFromAssembly(assembly);

        Assert.Contains(typeof(UnnamedDomainEvent), sut.DomainEvents);
        Assert.Same(sut, result);
    }
    
    [Fact]
    public void RegisterDomainEventsFromAssembly_ShouldNotAddIntegrationEvents_WhenAssemblyContainsIntegrationEvents()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetExecutingAssembly();

        var result = sut.RegisterDomainEventsFromAssembly(assembly);

        Assert.DoesNotContain(typeof(UnnamedIntegrationEvent), sut.IntegrationEvents);
    }

    [Fact]
    public void RegisterDomainEventsFromAssembly_ShouldThrowException_WhenDuplicateEventNameFound()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetAssembly(typeof(DomainEventOne))!;

        var exception = Assert.Throws<InvalidOperationException>(() => sut.RegisterDomainEventsFromAssembly(assembly));
        
        Assert.Equal("Duplicate event with name DomainEventOne", exception.Message);
    }

    [Fact]
    public void RegisterDomainAndIntegrationEventsFromAssembly_ShouldThrowException_WhenDuplicateEventNameFound()
    {
        //Domain event first
        var sut = CreateSut();
        var assembly = Assembly.GetAssembly(typeof(EventOne))!;
        
        sut.RegisterIntegrationEventsFromAssembly(assembly);
        
        var exception = Assert.Throws<InvalidOperationException>(() => sut.RegisterDomainEventsFromAssembly(assembly));
        Assert.Equal("Duplicate event with name EventOne", exception.Message);
        
        sut = CreateSut();
        
        sut.RegisterDomainEventsFromAssembly(assembly);
        
        exception = Assert.Throws<InvalidOperationException>(() => sut.RegisterIntegrationEventsFromAssembly(assembly));
        Assert.Equal("Duplicate event with name EventOne", exception.Message);
    }
    
    [Fact]
    public void RegisterHandlersFromAssembly_ShouldRegisterIntegrationEventHandler_WhenAssemblyContainsHandler()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetExecutingAssembly();

        var result = sut.RegisterHandlersFromAssembly(assembly);

        Assert.Contains(typeof(UnnamedIntegrationEventHandler), sut.IntegrationEventHandlers);
        Assert.Same(sut, result);
    }
    
    [Fact]
    public void RegisterHandlersFromAssembly_ShouldRegisterIntegrationEventHandlerInServiceCollection_WhenAssemblyContainsHandler()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetExecutingAssembly();

        var result = sut.RegisterHandlersFromAssembly(assembly);

        Assert.Contains(sut.Services, d => d.ServiceType == typeof(UnnamedIntegrationEventHandler));
    }
    
    [Fact]
    public void RegisterHandlersFromAssembly_ShouldRegisterDomainEventHandler_WhenAssemblyContainsHandler()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetExecutingAssembly();

        sut.RegisterHandlersFromAssembly(assembly);

        Assert.Contains(typeof(UnnamedDomainEventHandler), sut.DomainEventHandlers);
    }
    
    [Fact]
    public void RegisterHandlersFromAssembly_ShouldRegisterDomainEventHandlerInServiceCollection_WhenAssemblyContainsHandler()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetExecutingAssembly();

        var result = sut.RegisterHandlersFromAssembly(assembly);

        Assert.Contains(sut.Services, d => d.ServiceType == typeof(UnnamedDomainEventHandler));
    }
    
    [Theory]
    [InlineData(typeof(DomainEventHandlerOne), "DomainEventHandlerOne")]
    [InlineData(typeof(EventHandlerOne), "EventHandlerOne")]
    public void RegisterHandlersFromAssembly_ShouldThrowException_WhenDuplicateEventHandlerNameFound(Type eventHandlerType, string duplicateHandlerName)
    {
        var sut = CreateSut();
        var assembly = Assembly.GetAssembly(eventHandlerType)!;

        var exception = Assert.Throws<InvalidOperationException>(() => sut.RegisterHandlersFromAssembly(assembly));
        
        Assert.Equal($"Duplicate event handler with name {duplicateHandlerName}", exception.Message);
    }

    [Fact]
    public void RegisterHandlersFromAssembly_ShouldThrowException_WhenDomainEventHandlerHasForApplicationAttribute()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetAssembly(typeof(InvalidDomainEventHandler))!;
        
        var exception = Assert.Throws<InvalidOperationException>(() => sut.RegisterHandlersFromAssembly(assembly));

        Assert.Equal("Cannot set application name for InvalidDomainEventHandler since it is a domain event handler.", exception.Message);
    }
    
    [Fact]
    public void RegisterHandlersFromAssembly_ShouldThrowException_WhenIntegrationEventHandlerHasNoForApplicationAttribute()
    {
        var sut = CreateSut();
        var assembly = Assembly.GetAssembly(typeof(InvalidIntegrationEventHandler))!;
        
        var exception = Assert.Throws<InvalidOperationException>(() => sut.RegisterHandlersFromAssembly(assembly));

        Assert.Equal($"Integration event handler InvalidIntegrationEventHandler is missing its [ForApplication] attribute.", exception.Message);
    }
}