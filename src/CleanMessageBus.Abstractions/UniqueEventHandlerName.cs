namespace CleanMessageBus.Abstractions;

/// <summary>
/// A globally unique name for an event handler
/// </summary>
/// <param name="ApplicationName">Name of the application that handles the event</param>
/// <param name="EventHandlerName">Name of the event handler</param>
public record UniqueEventHandlerName(string ApplicationName, string EventHandlerName);