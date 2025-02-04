namespace vAIIS.SDK.Weather;

/// <summary>
/// Arguments of <see cref="IWeatherProvider.MetarUpdated"/>
/// </summary>
public class MetarUpdateEventHandlerArgs
{
    #region Public Constructors

    public MetarUpdateEventHandlerArgs()
    {
        Data = [];
    }

    public MetarUpdateEventHandlerArgs(IEnumerable<string> data)
    {
        Data = data;
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// A group of METAR strings
    /// </summary>
    public IEnumerable<string> Data { get; set; }

    #endregion Public Properties
}