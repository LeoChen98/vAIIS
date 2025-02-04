using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace vAIIS.SDK.Weather;

/// <summary>
/// METAR data
/// </summary>
public partial class MetarData
{
    #region Public Fields

    /// <summary>
    /// Airport ICAO code
    /// </summary>
    public string AirportICAO;

    /// <summary>
    /// A flag indicates whether the METAR is automatic, default is false.
    /// </summary>
    public bool Auto = false;

    /// <summary>
    /// A flag indicates whether the METAR is CAVOK, default is false.
    /// </summary>
    public bool CAVOK = false;

    /// <summary>
    /// Cloud layers
    /// </summary>
    public List<MetarCloudData> Cloud;

    /// <summary>
    /// Weather condition
    /// </summary>
    public string Condition;

    /// <summary>
    /// Raw report of weather condition
    /// </summary>
    public string ConditionRaw;

    /// <summary>
    /// A flag indicates whether the METAR is corrected, default is false.
    /// </summary>
    public bool Correction = false;

    /// <summary>
    /// Publish time of the METAR
    /// </summary>
    public DateTime PublishTime;

    /// <summary>
    /// QNH
    /// </summary>
    public int QNH;

    /// <summary>
    /// The unit of QNH, default is <see cref="QNHUnits.hPa"/>
    /// </summary>
    public QNHUnits QNHUnit = QNHUnits.hPa;

    /// <summary>
    /// Raw report of the METAR.
    /// </summary>
    public string Raw;

    /// <summary>
    /// Remarks in the METAR(raw)
    /// </summary>
    public string Remarks;

    /// <summary>
    /// Recent weather phenomena, RE parts.(raw)
    /// </summary>
    public string REs;

    /// <summary>
    /// A flag indicates whether the report is SPECI, default is false.
    /// </summary>
    public bool Special = false;

    /// <summary>
    /// Temperature data
    /// </summary>
    public MetarTemperatureData Temperature;

    /// <summary>
    /// Trends, BECMG, TEMPO parts.
    /// </summary>
    public string Trend;

    /// <summary>
    /// Visibility data
    /// </summary>
    public MetarVisibilityData Visibility;

    /// <summary>
    /// Wind data
    /// </summary>
    public MetarWindData Wind;

    /// <summary>
    /// Windshear data
    /// </summary>
    public MetarWindshearData Windshear;

    #endregion Public Fields

    #region Public Constructors

    public MetarData()
    {
        AirportICAO = string.Empty;
        Condition = string.Empty;
        ConditionRaw = string.Empty;
        Raw = string.Empty;
        Remarks = string.Empty;
        REs = string.Empty;
        Trend = string.Empty;

        Cloud = [];
        Temperature = new MetarTemperatureData();
        Visibility = new MetarVisibilityData();
        Wind = new MetarWindData();
        Windshear = new MetarWindshearData();
    }

    public MetarData(string raw) : this()
    {
        Raw = raw;
    }

    #endregion Public Constructors

    #region Public Enums

    /// <summary>
    /// QNH units
    /// </summary>
    public enum QNHUnits
    {
        /// <summary>
        /// hPa
        /// </summary>
        hPa,

        /// <summary>
        /// inHg, altimeter
        /// </summary>
        inHg
    }

    #endregion Public Enums
}