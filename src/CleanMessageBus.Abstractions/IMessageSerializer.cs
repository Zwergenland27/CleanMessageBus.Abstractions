namespace CleanMessageBus.Abstractions;

/// <summary>
/// Serializes and deserializes event payloads.
/// </summary>
public interface IMessageSerializer
{
    /// <summary>
    /// Content type of the serialized bytespan
    /// </summary>
    string ContentType { get; }

    /// <summary>
    /// Serializes <paramref name="value"/> as <paramref name="type"/>.
    /// </summary>
    ReadOnlyMemory<byte> Serialize(object value, Type type);

    /// <summary>
    /// Deserializes <paramref name="data"/> to <paramref name="type"/>.
    /// </summary>
    object Deserialize(ReadOnlyMemory<byte> data, Type type);
}