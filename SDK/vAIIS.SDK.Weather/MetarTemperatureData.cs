using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vAIIS.SDK.Weather;

/// <summary>
/// Temperature data
/// </summary>
public class MetarTemperatureData
{
    #region Public Fields

    /// <summary>
    /// Dewpoint in Celsius
    /// </summary>
    public int Dewpoint;

    /// <summary>
    /// Raw report of temperature
    /// </summary>
    public string Raw = string.Empty;

    /// <summary>
    /// Temperature in Celsius
    /// </summary>
    public int Temperature;

    #endregion Public Fields
}