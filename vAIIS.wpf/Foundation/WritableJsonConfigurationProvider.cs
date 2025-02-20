using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;

namespace vAIIS.Wpf.Foundation;
public class WritableJsonConfigurationProvider : JsonConfigurationProvider
{
    private readonly string _filePath;

    /// <summary>
    /// Initializes a new <see cref="WritableJsonConfigurationProvider"/>.
    /// </summary>
    /// <param name="source"><see cref="JsonConfigurationSource"/></param>
    /// <exception cref="ArgumentException"><paramref name="source"/>.Path should not be empty</exception>
    /// <exception cref="ArgumentNullException"><paramref name="source"/>.Path should not be <see langword="null"/></exception>
    public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source)
    {
        ArgumentNullException.ThrowIfNull(source.Path, nameof(source.Path));
        ArgumentException.ThrowIfNullOrWhiteSpace(source.Path, nameof(source.Path));
        _filePath = source.Path;
    }

    public override void Set(string key, string? value)
    {
        base.Set(key, value);
        Save();
    }

    public void Save()
    {
        var data = new Dictionary<string, string?>(Data, StringComparer.OrdinalIgnoreCase);
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }
}