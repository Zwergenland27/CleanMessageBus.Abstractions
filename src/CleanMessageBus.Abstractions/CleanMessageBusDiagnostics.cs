using System.Diagnostics;

namespace CleanMessageBus.Abstractions;

/// <summary>
/// Names of the <see cref="System.Diagnostics.ActivitySource"/> and <see cref="System.Diagnostics.Metrics.Meter"/>
/// used by CleanMessageBus. Register them in OpenTelemetry via <c>AddSource</c> and <c>AddMeter</c>.
/// </summary>
public static class CleanMessageBusDiagnostics
{
    /// <summary>
    /// Name of the activity source.
    /// </summary>
    public const string SourceName = "CleanMessageBus";

    /// <summary>
    /// Name of the meter.
    /// </summary>
    public const string MeterName = "CleanMessageBus";

    /// <summary>
    /// Shared activity source for all CleanMessageBus packages.
    /// </summary>
    public static ActivitySource ActivitySource { get; } =
        new(SourceName, typeof(CleanMessageBusDiagnostics).Assembly.GetName().Version?.ToString());
}