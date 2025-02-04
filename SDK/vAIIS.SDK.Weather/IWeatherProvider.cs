using vAIIS.SDK.Shared;

namespace vAIIS.SDK.Weather;

/// <summary>
/// Interface for weather providers
/// </summary>
public interface IWeatherProvider
{
    #region Public Events

    /// <summary>
    /// Event of AWOS updated
    /// </summary>
    public event EventHandler<AwosUpdateEnventHandlerArgs> AwosUpdated;

    /// <summary>
    /// Event of METAR updated
    /// </summary>
    public event EventHandler<MetarUpdateEventHandlerArgs> MetarUpdated;

    /// <summary>
    /// Event of TAF updated
    /// </summary>
    public event EventHandler<TafUpdateEventHandlerArgs> TafUpdated;

    #endregion Public Events

    #region Public Properties

    /// <summary>
    /// A flag indicates if AWOS report is supported by this provider.
    /// </summary>
    public bool IsAwosSupported { get; }

    /// <summary>
    /// A flag indicates if TAF report is supported by this provider.
    /// </summary>
    public bool IsTafSupported { get; }

    /// <summary>
    /// Interval of refreshing weather from the API
    /// </summary>
    public int RefreshInterval { get; set; }

    /// <summary>
    /// List of the airports subscribed
    /// </summary>
    public IList<string> SubscribedAirports { get; init; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Get AWOS report
    /// </summary>
    /// <param name="icao">ICAO code of the airport, e.g. ZBAA</param>
    /// <param name="runway">The runway number</param>
    /// <returns>AWOS data in <see cref="AwosData"/></returns>
    /// <exception cref="NotImplementedException"/>
    public AwosData GetAwos(string icao, string runway)
    {
        throw new NotImplementedException("AWOS is not supported by this provider.");
    }

    /// <summary>
    /// Get METAR report
    /// </summary>
    /// <param name="icao">ICAO code of the airport, e.g. ZBAA</param>
    /// <returns>METAR report in plain text.</returns>
    public string GetMetar(string icao);

    /// <summary>
    /// Get METAR report in VATSIM format
    /// </summary>
    /// <param name="icao">ICAO code of the airport, e.g. ZBAA</param>
    /// <returns>METAR report in VATSIM format plain text.</returns>
    public string GetMetarInVatsimFormat(string icao);

    /// <summary>
    /// Get TAF report
    /// </summary>
    /// <param name="icao">ICAO code of the airport, e.g. ZBAA</param>
    /// <returns>TAF report in plain text.</returns>
    /// <exception cref="NotImplementedException"/>
    public string GetTaf(string icao)
    {
        throw new NotImplementedException("TAF is not supported by this provider.");
    }

    /// <summary>
    /// Startup the provider. The timer or scheduler should be started here.
    /// </summary>
    public void Start();

    /// <summary>
    /// Stop the provider. The timer or scheduler should be stopped but not disposed here.
    /// </summary>
    public void Stop();

    /// <summary>
    /// Subscribe weather infomation of specific airport
    /// </summary>
    /// <param name="airport">ICAO code of the airport, e.g. ZBAA</param>
    public void Subscribe(string airport);

    /// <summary>
    /// Subscribe weather infomation of specific airport
    /// </summary>
    /// <param name="airports">ICAO code of the airports, e.g. ZBAA</param>
    public void SubscribeMany(IEnumerable<string> airports)
    {
        foreach (string airport in airports)
        {
            Subscribe(airport);
        }
    }

    /// <summary>
    /// Cancel the subscribe of specific airport
    /// </summary>
    /// <param name="icao">ICAO code of the airport, e.g. ZBAA</param>
    public void Unsubscribe(string icao);

    /// <summary>
    /// Cancel all of the subscriptions
    /// </summary>
    public void UnsubscribeAll()
    {
        foreach (string airport in SubscribedAirports)
        {
            Unsubscribe(airport);
        }
    }

    /// <summary>
    /// Cancel the subscribe of specific airports
    /// </summary>
    /// <param name="airports">ICAO code of the airports, e.g. ZBAA</param>
    public void UnsubscribeMany(IEnumerable<string> airports)
    {
        foreach (string airport in airports)
        {
            Unsubscribe(airport);
        }
    }

    /// <summary>
    /// Force update the weather
    /// </summary>
    public void UpdateWeather();

    #endregion Public Methods
}