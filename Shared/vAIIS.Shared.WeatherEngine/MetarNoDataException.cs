namespace vAIIS.Shared.WeatherEngine;

/// <summary>
/// The exception that is thrown when no METAR data
/// </summary>
public class MetarNoDataException : Exception
{
    #region Public Constructors

    public MetarNoDataException() : base("no METAR data")
    {
    }

    public static void ThrowIfNoData(string metar)
    {
        if (string.IsNullOrWhiteSpace(metar) || metar == "No Data")
        {
            throw new MetarNoDataException();
        }
    }

    #endregion Public Constructors
}