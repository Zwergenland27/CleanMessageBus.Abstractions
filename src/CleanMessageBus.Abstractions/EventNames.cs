using System.Collections.Concurrent;
using System.Reflection;

namespace CleanMessageBus.Abstractions;

/// <summary>
/// Resolves the routing name of event types.
/// </summary>
public static class EventNames
{
    private static readonly ConcurrentDictionary<Type, string> Cache = new();

    /// <summary>
    /// Returns the event name of <typeparamref name="TEvent"/>.
    /// </summary>
    public static string For<TEvent>() => For(typeof(TEvent));

    /// <summary>
    /// Returns the event name of <paramref name="eventType"/>: the value of its
    /// </summary>
    public static string For(Type eventType)
    {
        ArgumentNullException.ThrowIfNull(eventType);

        return Cache.GetOrAdd(
            eventType,
            static type => type.GetCustomAttribute<EventNameAttribute>(inherit: false)?.Name ?? type.Name);
    }
}