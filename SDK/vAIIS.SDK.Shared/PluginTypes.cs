namespace vAIIS.SDK.Shared;

/// <summary>
/// Enum for plugin types.
/// </summary>
[Flags]
public enum PluginTypes
{
    /// <summary>
    /// Default plugin type.
    /// </summary>
    Default,

    /// <summary>
    /// Weather provider plugin type.
    /// </summary>
    WeatherProvider,

    /// <summary>
    /// Page provider plugin type which provides a page can be inserted into main window.
    /// </summary>
    PageProvider,
}