using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace vAIIS.Wpf.Foundation;

public class WritableJsonConfigurationSource : JsonConfigurationSource
{
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        return new WritableJsonConfigurationProvider(this);
    }
}