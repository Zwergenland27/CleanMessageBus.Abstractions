namespace CleanMessageBus.Abstractions.Attributes;

/// <summary>
/// Defines the application name for an integration event handler
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ForApplicationAttribute : Attribute
{
    /// <summary>
    /// Name of the application
    /// </summary>
    public string ApplicationName { get; }
    
    /// <summary>
    /// Defines the application name for an integration event handler
    /// </summary>
    /// <param name="applicationName">Name of the application</param>
    public ForApplicationAttribute(string applicationName)
    {
        ApplicationName = applicationName;
    }
}