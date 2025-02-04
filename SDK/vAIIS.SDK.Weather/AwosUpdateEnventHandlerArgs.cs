namespace vAIIS.SDK.Weather;

/// <summary>
/// Arguments of <see cref="IWeatherProvider.AwosUpdated"/>
/// </summary>
public class AwosUpdateEnventHandlerArgs
{
    #region Public Constructors

    public AwosUpdateEnventHandlerArgs(IEnumerable<AwosData> data)
    {
        Data = data;
    }

    public AwosUpdateEnventHandlerArgs()
    {
        Data = [];
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// A group of <see cref="AwosData"/>
    /// </summary>
    public IEnumerable<AwosData> Data { get; init; }

    #endregion Public Properties
}