namespace vAIIS.SDK.Weather;

/// <summary>
/// Wind data
/// </summary>
public class MetarWindData
{
    #region Public Fields

    /// <summary>
    /// Wind direction
    /// </summary>
    public int Direction;

    /// <summary>
    /// Guesting wind speed
    /// </summary>
    public int Guest;

    /// <summary>
    /// A flag indicates whether the wind speed is in knots, default is false (unit is meters).
    /// </summary>
    public bool IsKnots;

    /// <summary>
    /// A flag indicates whether the wind is variable, default is false.
    /// </summary>
    public bool IsVariable = false;

    /// <summary>
    /// Raw report of wind data
    /// </summary>
    public string Raw = string.Empty;

    /// <summary>
    /// Wind speed, default is -1 meaning error.
    /// </summary>
    public int Speed = -1;

    /// <summary>
    /// Ending direction of variable wind
    /// </summary>
    public int VarDirectionEnd;

    /// <summary>
    /// Starting direction of variable wind
    /// </summary>
    public int VarDirectionStart;

    #endregion Public Fields
}