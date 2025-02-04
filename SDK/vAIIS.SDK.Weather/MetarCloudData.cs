using System.Diagnostics;
using System.Text.RegularExpressions;

namespace vAIIS.SDK.Weather;

/// <summary>
/// Cloud data
/// </summary>
public partial class MetarCloudData
{
    #region Public Fields

    /// <summary>
    /// Cloud height, in feet, defalut is 0.
    /// </summary>
    public int Height = 0;

    /// <summary>
    /// Cloudage, default is <see cref="Cloudage.skyclear"/>
    /// </summary>
    public Cloudage Layer = Cloudage.skyclear;

    /// <summary>
    /// Raw report
    /// </summary>
    public required string Raw;

    /// <summary>
    /// Cloud type, default is <see cref="CloudType.Cloud"/>
    /// </summary>
    public CloudType Type = CloudType.Cloud;

    #endregion Public Fields

    #region Private Fields

    static readonly Regex reg_cloud = CloudRegex();

    #endregion Private Fields

    #region Public Enums

    /// <summary>
    /// Cloudage
    /// </summary>
    public enum Cloudage
    {
        /// <summary>
        /// No cloud data
        /// </summary>
        no_cloud_data = -1,

        /// <summary>
        /// Sky clear, SKC
        /// </summary>
        skyclear = 0,

        /// <summary>
        /// No significant cloud, NSC
        /// </summary>
        no_significant_cloud = 1,

        /// <summary>
        /// Few cloud, FEW
        /// </summary>
        few = 2,

        /// <summary>
        /// Scattered cloud, SCT
        /// </summary>
        scattered = 3,

        /// <summary>
        /// Borken cloud, BKN
        /// </summary>
        broken = 4,

        /// <summary>
        /// Overcast, OVC
        /// </summary>
        overcast = 5
    }

    /// <summary>
    /// Cloud type
    /// </summary>
    public enum CloudType
    {
        /// <summary>
        /// 普通云，CLD
        /// </summary>
        Cloud = 0,

        /// <summary>
        /// 卷云，CI
        /// </summary>
        Cirrus = 1,

        /// <summary>
        /// 卷积云，CC
        /// </summary>
        Cirrocumulus = 2,

        /// <summary>
        /// 卷层云，CS
        /// </summary>
        Cirrostratus = 3,

        /// <summary>
        /// 高积云，AC
        /// </summary>
        Altocumulus = 4,

        /// <summary>
        /// 高层云，AS
        /// </summary>
        Altostratus = 5,

        /// <summary>
        /// 雨层云，NS
        /// </summary>
        Nimbostratus = 6,

        /// <summary>
        /// 层积云，SC
        /// </summary>
        Stratocumulus = 7,

        /// <summary>
        /// 层云，ST
        /// </summary>
        Stratus = 8,

        /// <summary>
        /// 积云，CU
        /// </summary>
        Cumulus = 9,

        /// <summary>
        /// 塔状积云，TCU
        /// </summary>
        Towering_Cumulus = 10,

        /// <summary>
        /// 积雨云，CB
        /// </summary>
        Cumulonimbus = 11,

        /// <summary>
        /// 浓密上层云，DUC
        /// </summary>
        Dense_Upper_Cloud = 12,

        /// <summary>
        /// 积状云，CUF
        /// </summary>
        Cumuliform = 13,

        /// <summary>
        /// 层状云，STF
        /// </summary>
        Stratiform = 14
    }

    #endregion Public Enums

    #region Public Methods

    /// <summary>
    /// Parse cloud data
    /// </summary>
    /// <param name="data">Raw cloud report, single group.</param>
    /// <returns><see cref="MetarCloudData"/></returns>
    [DebuggerStepThrough]
    public static MetarCloudData Parse(string data)
    {
        MetarCloudData r = new() { Raw = data };

        Match match_cloud = reg_cloud.Match(data);

        if (match_cloud.Success)
        {
            r.Layer = match_cloud.Groups["cloud_layer"].Value switch
            {
                "FEW" => Cloudage.few,
                "SCT" => Cloudage.scattered,
                "BKN" => Cloudage.broken,
                "OVC" => Cloudage.overcast,
                "NSC" => Cloudage.no_significant_cloud,
                "SKC" => Cloudage.skyclear,
                _ => Cloudage.no_cloud_data,
            };
            if (match_cloud.Groups["cloud_ceiling"].Success)
            {
                if (match_cloud.Groups["cloud_ceiling"].Value == "///")
                {
                    r.Height = -1;
                }
                else
                {
                    r.Height = int.Parse(match_cloud.Groups["cloud_ceiling"].Value) * 100;
                }
            }

            if (match_cloud.Groups["cloud_type"].Success)
            {
                r.Type = match_cloud.Groups["cloud_type"].Value switch
                {
                    "CI" => CloudType.Cirrus,
                    "CC" => CloudType.Cirrocumulus,
                    "CS" => CloudType.Cirrostratus,
                    "AC" => CloudType.Altocumulus,
                    "AS" => CloudType.Altostratus,
                    "NS" => CloudType.Nimbostratus,
                    "SC" => CloudType.Stratocumulus,
                    "ST" => CloudType.Stratus,
                    "CU" => CloudType.Cumulus,
                    "TCU" => CloudType.Towering_Cumulus,
                    "CB" => CloudType.Cumulonimbus,
                    "DUC" => CloudType.Dense_Upper_Cloud,
                    "CUF" => CloudType.Cumuliform,
                    "STF" => CloudType.Stratiform,
                    _ => CloudType.Cloud,
                };
            }
        }

        return r;
    }

    /// <summary>
    /// Parse cloud data
    /// </summary>
    /// <param name="c">Regex result of raw cloud report.</param>
    /// <param name="rs">Reference of METAR data instance to insert the decode result.</param>
    /// <returns><see cref="MetarCloudData"/></returns>
    [DebuggerStepThrough]
    public static MetarCloudData Parse(Match c, ref MetarData rs)
    {
        MetarCloudData r = new() { Raw = c.Value };
        switch (c.Groups[1].Value)
        {
            case "VV":
                rs.Visibility.VerticalVisibility = int.Parse(c.Groups[2].Value);
                break;

            case "FEW":
                r.Layer = Cloudage.few;
                break;

            case "SCT":
                r.Layer = Cloudage.scattered;
                break;

            case "BKN":
                r.Layer = Cloudage.broken;
                break;

            case "OVC":
                r.Layer = Cloudage.overcast;
                break;

            case "NSC":
                r.Layer = Cloudage.no_significant_cloud;
                break;

            case "SKC":
                r.Layer = Cloudage.skyclear;
                break;

            default:
                r.Layer = Cloudage.no_cloud_data;
                break;
        }

        if (c.Groups[2].Success)
        {
            if (c.Groups[2].Value == "///")
            {
                r.Height = -1;
            }
            else
                r.Height = int.Parse(c.Groups[2].Value) * 100;
        }

        if (c.Groups[3].Success)
        {
            r.Type = c.Groups[3].Value switch
            {
                "CI" => CloudType.Cirrus,
                "CC" => CloudType.Cirrocumulus,
                "CS" => CloudType.Cirrostratus,
                "AC" => CloudType.Altocumulus,
                "AS" => CloudType.Altostratus,
                "NS" => CloudType.Nimbostratus,
                "SC" => CloudType.Stratocumulus,
                "ST" => CloudType.Stratus,
                "CU" => CloudType.Cumulus,
                "TCU" => CloudType.Towering_Cumulus,
                "CB" => CloudType.Cumulonimbus,
                "DUC" => CloudType.Dense_Upper_Cloud,
                "CUF" => CloudType.Cumuliform,
                "STF" => CloudType.Stratiform,
                _ => CloudType.Cloud,
            };
        }

        return r;
    }

    #endregion Public Methods

    #region Private Methods

    [GeneratedRegex(@"(?<cloud_layer>[A-Z]{3})(?<cloud_ceiling>\d{3}|///)?(?<cloud_type>\S+)?")]
    private static partial Regex CloudRegex();

    #endregion Private Methods
}
