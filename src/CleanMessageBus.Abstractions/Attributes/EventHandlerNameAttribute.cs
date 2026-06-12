namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Defines custom event handler name
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class EventHandlerNameAttribute: Attribute
{
    /// <summary>
    /// Custom name of the event handler
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Defines custom event handler name
    /// </summary>
    /// <param name="name">Event name</param>
    public EventHandlerNameAttribute(string name)
    {
        Name = name;
    }
}