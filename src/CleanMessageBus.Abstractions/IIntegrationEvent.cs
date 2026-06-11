using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace CleanMessageBus.Abstractions;


/// <summary>
/// Marker interface for integration events
/// </summary>
public interface IIntegrationEvent : IRequest
{
    /// <summary>
    /// Id of the event
    /// </summary>
    Guid Id { get; }
    
    /// <summary>
    /// Name of the event
    /// </summary>
    [JsonIgnore]
    string EventName { get; }
}