namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Defines custom event name
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class EventNameAttribute : Attribute
{
    /// <summary>
    /// Custom name of the event
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Defines custom event name
    /// </summary>
    /// <param name="name">Event name</param>
    public EventNameAttribute(string name)
    {
        Name = name;
    }
}