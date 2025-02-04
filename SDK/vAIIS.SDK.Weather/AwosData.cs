namespace vAIIS.SDK.Weather;

/// <summary>
/// AWOS data
/// </summary>
public class AwosData
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AwosData"/> class.
    /// </summary>
    /// <param name="airport">The airport ICAO code.</param>
    /// <param name="runway">The runway number.</param>
    /// <param name="windBearing">The wind bearing in degrees.</param>
    /// <param name="windSpeed">The wind speed.</param>
    /// <param name="rvr">The runway visual range in meters.</param>
    /// <param name="windSpeedUnit">
    /// The unit of wind speed. Default is meters per second (m/s). More details see <see cref="WindSpeedUnits"/>
    /// </param>
    public AwosData(string airport, string runway, int windBearing, int windSpeed, int rvr, WindSpeedUnits windSpeedUnit = WindSpeedUnits.MPS)
    {
        Airport = airport;
        Runway = runway;
        WindBearing = windBearing;
        WindSpeed = windSpeed;
        RVR = rvr;
        WindSpeedUnit = windSpeedUnit;
    }

    #endregion Public Constructors

    #region Public Enums

    /// <summary>
    /// AWOS wind speed units.
    /// </summary>
    public enum WindSpeedUnits
    {
        /// <summary>
        /// Meter(s) per second, m/s, default unit.
        /// </summary>
        MPS,

        /// <summary>
        /// Knot(s) per second, kt/s.
        /// </summary>
        KTS
    }

    #endregion Public Enums

    #region Public Properties

    /// <summary>
    /// Gets or sets the airport ICAO code.
    /// </summary>
    public string Airport { get; set; }

    /// <summary>
    /// Gets or sets the runway number.
    /// </summary>
    public string Runway { get; set; }

    /// <summary>
    /// Gets or sets the runway visual range in meters.
    /// </summary>
    public int RVR { get; set; }

    /// <summary>
    /// Gets or sets the wind bearing in degrees.
    /// </summary>
    public int WindBearing { get; set; }

    /// <summary>
    /// Gets or sets the wind speed.
    /// </summary>
    public int WindSpeed { get; set; }

    /// <summary>
    /// Gets or sets the unit of wind speed. Default is meters per second (m/s). More details
    /// see <see cref="WindSpeedUnits"/>
    /// </summary>
    public WindSpeedUnits WindSpeedUnit { get; set; } = WindSpeedUnits.MPS;

    #endregion Public Properties
}