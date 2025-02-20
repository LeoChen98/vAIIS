using System.Runtime.Loader;
using Microsoft.Extensions.Logging;
using vAIIS.SDK.Shared;

namespace vAIIS.Wpf.Foundation;

public class PluginManager
{
    private readonly ILogger _logger;

    /// <summary>
    /// Provides a list of loaded plugins in the form of <see cref="IPluginInfo"/>.
    /// </summary>
    public IEnumerable<IPluginInfo> PluginInfos => _pluginList.Values.Select(x => x.info);

    private readonly Dictionary<string, (IPluginInfo info, AssemblyLoadContext context)> _pluginList = [];

    public PluginManager(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Load a plugin from a file path
    /// </summary>
    /// <param name="path">A path contains one or more <see langword="class"/> inherit from <see cref="IPluginInfo"/></param>
    /// <returns><see langword="bool"/>, <see langword="true"/> means the plugin has loaded successfully, <see langword="false"/> means not.</returns>
    public bool LoadPlugin(string path)
    {
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

                _pluginList[plugin.Name] = (plugin, context);
                _logger.LogInformation($"Plugin {plugin.Name} loaded from {path}");
            }

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error loading plugin from {path}");
            return false;
        }
    }

    /// <summary>
    /// Unload a plugin by name
    /// </summary>
    /// <param name="pluginName">Plugin's name</param>
    /// <returns>
    /// <para><see langword="bool?"/>, </para>
    /// <para><see langword="true"/> means unloading is success,</para>
    /// <para><see langword="false"/> means unloading is blocked by some reason,</para>
    /// <para><see langword="null"/> means unloading has not executed because instance is not exist.</para>
    /// </returns>
    public bool? UnloadPlugin(string pluginName)
    {
        if (_pluginList.TryGetValue(pluginName, out var plugin))
        {
            if (plugin.info.Type == PluginTypes.WeatherProvider) // TODO: Check if plugin is in use
            {
                return false;
            }

            plugin.context.Unload();
            _pluginList.Remove(pluginName);
            _logger.LogInformation($"Plugin {pluginName} unloaded");
            return true;
        }
        else
        {
            _logger.LogWarning($"Plugin {pluginName} not found in loaded contexts");
            return null;
        }
    }
}