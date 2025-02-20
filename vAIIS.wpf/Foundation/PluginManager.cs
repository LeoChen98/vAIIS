using System.IO;
using System.Runtime.Loader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using vAIIS.SDK.Shared;

namespace vAIIS.Wpf.Foundation;

public class PluginManager
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;

    /// <summary>
    /// Provides a list of loaded plugins in the form of <see cref="IPluginInfo"/>.
    /// </summary>
    public IEnumerable<IPluginInfo> PluginInfos => _pluginList.Values.Select(x => x.info);

    private readonly Dictionary<string, (IPluginInfo info, AssemblyLoadContext context, string path)> _pluginList = [];

    public PluginManager(ILogger logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        
        // Load plugins from config if exists
        if(_config.GetSection("Plugins").Exists() && _config.GetSection("Plugins").GetChildren().Any())
        {
            var plugins = _config.GetSection("Plugins").Get<Dictionary<string,IEnumerable<(string,IPluginInfo)>>>();
            foreach (var plugin in plugins.Keys)
            {
                foreach (var (path, _) in plugins[plugin])
                {
                    LoadPlugin(path, out _);
                }
            }
        }
    }

    /// <summary>
    /// Load a new plugin from a file path
    /// </summary>
    /// <param name="path">A path contains one or more <see langword="class"/> inherit from <see cref="IPluginInfo"/></param>
    /// <param name="errorInfo">Error information if loading failed</param>
    /// <returns><see langword="bool"/>, <see langword="true"/> means the plugin has loaded successfully, <see langword="false"/> means not.</returns>
    public bool LoadNewPlugin(string path, out string? errorInfo)
    {
        bool rs = LoadNewPlugin(path, out errorInfo);
        if (rs)
        {
            SavePluginList();
        }
        return rs;
    }

    /// <summary>
    /// Load a plugin from a file path
    /// </summary>
    /// <param name="path">A path contains one or more <see langword="class"/> inherit from <see cref="IPluginInfo"/></param>
    /// <param name="errorInfo">Error information if loading failed</param>
    /// <returns><see langword="bool"/>, <see langword="true"/> means the plugin has loaded successfully, <see langword="false"/> means not.</returns>
    public bool LoadPlugin(string path, out string? errorInfo)
    {
        if (!File.Exists(path))
        {
            _logger.LogWarning($"Path {path} does not exist");
            errorInfo = "Path does not exist";
            return false;
        }
        
        try
        {
            var context = new AssemblyLoadContext(path, true);
            var assembly = context.LoadFromAssemblyPath(path);
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.GetInterface(nameof(IPluginInfo)) == null) continue;
                var plugin = (IPluginInfo?)Activator.CreateInstance(type);
                if (plugin == null)
                {
                    _logger.LogWarning($"Plugin {type.Name} could not be created from {path}");
                    continue;
                }

                if (_pluginList.ContainsKey(plugin.Name))
                {
                    _logger.LogWarning($"Plugin {plugin.Name} already loaded from {path}");
                    continue;
                }

                _pluginList[plugin.Name] = (plugin, context, path);
                _logger.LogInformation($"Plugin {plugin.Name} loaded from {path}");
            }
            
            errorInfo = null;
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error loading plugin from {path}");
            errorInfo = $"Error loading plugin from {path}. {e.Message}";
            return false;
        }
    }

    /// <summary>
    /// Unload a plugin by name
    /// </summary>
    /// <param name="pluginName">Plugin's name</param>
    /// <param name="errorInfo">Error information if unloading failed</param>
    /// <returns>
    /// <para><see langword="bool?"/>, </para>
    /// <para><see langword="true"/> means unloading is success,</para>
    /// <para><see langword="false"/> means unloading is blocked by some reason,</para>
    /// <para><see langword="null"/> means unloading has not executed because instance is not exist.</para>
    /// </returns>
    public bool? UnloadPlugin(string pluginName, out string? errorInfo)
    {
        if (_pluginList.TryGetValue(pluginName, out var plugin))
        {
            if (plugin.info.Type == PluginTypes.WeatherProvider) // TODO: Check if plugin is in use
            {
                
                errorInfo = "Plugin is in use";
                return false;
            }

            plugin.context.Unload();
            _pluginList.Remove(pluginName);
            _logger.LogInformation($"Plugin {pluginName} unloaded");
            errorInfo = null;
            return true;
        }
        else
        {
            _logger.LogWarning($"Plugin {pluginName} not found in loaded contexts");
            errorInfo = "Plugin not found";
            return null;
        }
    }
    
    /// <summary>
    /// Save plugin list to configuration
    /// </summary>
    private void SavePluginList()
    {
        _config.GetSection("Plugins").Bind(_pluginList.Values.Select(x => (x.path, x.info)).GroupBy(e => e.path));
    }
}