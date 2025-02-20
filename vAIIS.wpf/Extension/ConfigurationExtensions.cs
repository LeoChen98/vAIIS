using Microsoft.Extensions.Configuration;
using vAIIS.Wpf.Foundation;

namespace vAIIS.Wpf.Extension;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder, string path, bool optional = false, bool reloadOnChange = false)
    {
        var source = new WritableJsonConfigurationSource
        {
            FileProvider = null,
            Path = path,
            Optional = optional,
            ReloadOnChange = reloadOnChange
        };
        builder.Add(source);
        return builder;
    }
}