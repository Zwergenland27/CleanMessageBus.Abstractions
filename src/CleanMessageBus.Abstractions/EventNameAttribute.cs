namespace CleanMessageBus.Abstractions;

/// <summary>
/// Overrides the name under which an event is published and routed.
/// Without this attribute the simple type name is used.
/// </summary>
/// <remarks>
/// The event name is part of the contract between publisher and consumers.
/// Use the attribute to keep the name stable when renaming or moving the type.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class EventNameAttribute : Attribute
{
    /// <summary>
    /// Creates the attribute.
    /// </summary>
    /// <param name="name">The event name. Must not be empty.</param>
    public EventNameAttribute(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
    }

    /// <summary>The event name.</summary>
    public string Name { get; }
}