namespace vAIIS.SDK.Weather;

/// <summary>
/// Arguments of <see cref="IWeatherProvider.TafUpdated"/>
/// </summary>
public class TafUpdateEventHandlerArgs
{
    #region Public Constructors

    public TafUpdateEventHandlerArgs()
    {
        Data = [];
    }

    public TafUpdateEventHandlerArgs(IEnumerable<string> data)
    {
        Data = data;
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// A group of TAF strings
    /// </summary>
    public IEnumerable<string> Data { get; set; }

    #endregion Public Properties
}