using Microsoft.Extensions.Logging;
using System.Drawing;

namespace vAIIS.SDK.Shared;

public abstract class PluginBase : IPluginInfo
{
    #region Private Fields
    /// <summary>
    /// Serilog logger
    /// </summary>
    private readonly ILogger _logger;

    #endregion Private Fields

    #region Public Constructors

    public PluginBase(ILogger logger)
    {
        this._logger = logger;
    }

    #endregion Public Constructors

    #region Public Properties

    public abstract string Author { get; }

    public abstract bool CanAutoUpdate { get; }

    public abstract string Copyright { get; }

    public abstract string Description { get; }

    public abstract Bitmap? Icon { get; }

    public abstract string Name { get; }

    public abstract PluginTypes Type { get; }

    public abstract string VersionString { get; }

    #endregion Public Properties
}