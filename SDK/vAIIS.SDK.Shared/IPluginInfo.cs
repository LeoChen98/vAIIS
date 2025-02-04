using System.Drawing;

namespace vAIIS.SDK.Shared;

/// <summary>
/// Interface for plugin information.
/// </summary>
public interface IPluginInfo
{
    #region Public Properties

    /// <summary>
    /// Author of the plugin.
    /// </summary>
    public string Author { get; }

    /// <summary>
    /// Flag to indicate if the plugin can auto update. If <see langword="true"/>, the plugin should
    /// implement <see cref="IPluginAutoUpdate"/>.
    /// </summary>
    public bool CanAutoUpdate { get; }

    /// <summary>
    /// Copyrights of the plugin.
    /// </summary>
    public string Copyright { get; }

    /// <summary>
    /// Description of the plugin.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Icon of the plugin. Suggest saving the icon in the project resources. Can be <see langword="null"/>.
    /// </summary>
    /// <example>
    /// <code>
    ///public Bitmap Icon { get; } =&gt; Properties.Resources.Icon;
    /// </code>
    /// </example>
    public Bitmap? Icon { get; }

    /// <summary>
    /// Name of the plugin.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// <see cref="PluginTypes">Type</see> of the plugin.
    /// </summary>
    public PluginTypes Type { get; }

    /// <summary>
    /// Version of the plugin.
    /// </summary>
    public string VersionString { get; }

    /// <summary>
    /// Version of the plugin.
    /// </summary>
    public Version? Version => Version.TryParse(VersionString, out var version) ? version : null;

    #endregion Public Properties
}