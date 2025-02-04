namespace vAIIS.SDK.Weather;

/// <summary>
/// Visibility data
/// </summary>
public class MetarVisibilityData
{
    #region Public Fields

    /// <summary>
    /// Primary visibility, default is 9999(more than 10km), when error is -1
    /// </summary>
    public int PrimaryVisibility = 9999;

    /// <summary>
    /// Unit of primary visibility, default is meters
    /// </summary>
    public string HorizontalVisibilityUnit = "m";

    /// <summary>
    /// RVR
    /// </summary>
    public Dictionary<string, RVRData>? RVR;

    /// <summary>
    /// Vertical visibility, default is 999
    /// </summary>
    public int VerticalVisibility = 999;

    #endregion Public Fields

    #region Public Classes

    /// <summary>
    /// RVR data
    /// </summary>
    public class RVRData
    {
        #region Public Fields

        /// <summary>
        /// Average RVR in 10min or lower RVR in 1min, default is 9999
        /// </summary>
        public int LowerVisibility = 9999;

        /// <summary>
        /// Lower exceed-range mark of RVR, default is <see cref="RVRPM.None"/>
        /// </summary>
        public RVRPM LPM = RVRPM.None;

        /// <summary>
        /// Trends of RVR, default is <see cref="RVRTrends.None"/>
        /// </summary>
        public RVRTrends Trends = RVRTrends.None;

        /// <summary>
        ///  Upper exceed-range mark of RVR, default is <see cref="RVRPM.None"/>
        /// </summary>
        public RVRPM UPM = RVRPM.None;

        /// <summary>
        /// Upper RVR in 1min, default is 9999
        /// </summary>
        public int UpperVisibility = 9999;

        #endregion Public Fields

        #region Public Enums

        /// <summary>
        /// Enumeration of RVR exceed-range marks
        /// </summary>
        public enum RVRPM
        {
            /// <summary>
            /// Exceeds lower bound, M in report
            /// </summary>
            less_than_ = -1,

            /// <summary>
            /// (default) Inapplicable.
            /// </summary>
            None = 0,

            /// <summary>
            /// Exceeds upper bound, P in report
            /// </summary>
            more_than_ = 1
        }

        /// <summary>
        /// Enumeration of RVR trends
        /// </summary>
        public enum RVRTrends
        {
            /// <summary>
            /// Decreasing, D
            /// </summary>
            decreasing = -1,

            /// <summary>
            /// (default) No change or unkown, N
            /// </summary>
            None = 0,

            /// <summary>
            /// Increasing, U
            /// </summary>
            increasing = 1
        }

        #endregion Public Enums
    }

    #endregion Public Classes
}
