using System.Diagnostics;

namespace vAIIS.Shared.WeatherEngine;

/// <summary>
/// Weather condition data decoder
/// </summary>
/// <remarks>
/// note: Written according to <see href="https://www.caac.gov.cn/XXGK/XXGK/GFXWJ/202112/P020220209588831109510.pdf">CAAC AP-117-TM-2021-01R2</see>
/// </remarks>
public class MetarWeatherConditionDecoder
{
    #region Public Methods

    /// <summary>
    /// Decode weather condition from raw METAR data
    /// </summary>
    /// <param name="raw">Raw report of weather condition(single group). Example: <c>+TSRA</c></param>
    /// <returns>Description of weather condition.</returns>
    [DebuggerStepThrough]
    public static string Parse(string raw)
    {
        raw = raw.Trim();
        string rs = "";

        if (raw.Contains('+'))
        {
            rs = "heavy ";
            raw = raw[1..];
        }
        else if (raw.Contains('-'))
        {
            rs = "light ";
            raw = raw[1..];
        }

        bool isContainVC = false;
        for (int i = 0; i < raw.Length; i += 2)
        {
            switch (raw.Substring(i, 2))
            {
                case "BC":
                    rs += "patches ";
                    break;

                case "BL":
                    rs += "blowing ";
                    break;

                case "BR":
                    rs += "mist";
                    break;

                case "DR":
                    rs += "drifting ";
                    break;

                case "DS":
                    rs += "duststorm";
                    break;

                case "DU":
                    rs += "wide spread dust";
                    break;

                case "DZ":
                    rs += "drizzle";
                    break;

                case "FC":
                    rs += "funnel clouds";
                    break;

                case "FG":
                    rs += "fog";
                    break;

                case "FU":
                    rs += "smoke";
                    break;

                case "FZ":
                    rs += "freezing ";
                    break;

                case "GR":
                    rs += "hail";
                    break;

                case "GS":
                    rs += "small hail";
                    break;

                case "HZ":
                    rs += "haze";
                    break;

                case "IC":
                    rs += "ice crystals";
                    break;

                case "MI":
                    rs += "shallow";
                    break;

                case "PE":
                    rs += "ice pellets";
                    break;

                case "PO":
                    rs += "dust or sandwhirls";
                    break;

                case "PR":
                    rs += "partial ";
                    break;

                case "RA":
                    rs += "rain";
                    break;

                case "SA":
                    rs += "sand";
                    break;

                case "SG":
                    rs += "snow grains";
                    break;

                case "SH":
                    rs += "showers ";
                    break;

                case "SN":
                    rs += "snow";
                    break;

                case "SQ":
                    rs += "squall";
                    break;

                case "SS":
                    rs += "sand storm";
                    break;

                case "TS":
                    rs += "thunderstorm ";
                    break;

                case "VA":
                    rs += "volcanic ash";
                    break;

                case "VC":
                    isContainVC = true;
                    break;

                case "WS":
                    rs += "windshear";
                    break;

                default:
                    rs += "";
                    break;
            }
        }

        if (isContainVC)
        {
            rs += " in the vicinity";
        }

        return rs;
    }

    #endregion Public Methods
}