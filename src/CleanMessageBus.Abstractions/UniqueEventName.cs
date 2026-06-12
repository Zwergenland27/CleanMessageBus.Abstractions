namespace CleanMessageBus.Abstractions;

/// <summary>
/// A globally unique name for an event
/// </summary>
/// <param name="ApplicationName">Name of the application the event occured in</param>
/// <param name="EventName">Name of the event</param>
public record UniqueEventName(string ApplicationName, string EventName);