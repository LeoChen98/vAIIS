using System.Text.RegularExpressions;
using vAIIS.SDK.Weather;

namespace vAIIS.Shared.WeatherEngine
{
    /// <summary>
    /// METAR decoder
    /// </summary>
    /// <remarks>
    /// note: Written according to <see href="https://www.caac.gov.cn/XXGK/XXGK/GFXWJ/202112/P020220209588831109510.pdf">CAAC AP-117-TM-2021-01R2</see>
    /// </remarks>
    public partial class MetarDecoder
    {
        #region Private Fields

        private static readonly Regex _reg_METAR = RegexMETAR();
        private static readonly Regex _reg_no_data = RegexNoData();
        private static readonly Regex _reg_REs = RegexREs();
        private static readonly Regex _reg_RVR = RegexRVR();
        private static readonly Regex _reg_WS = RegexWindShear();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Decode METAR/SPECI report
        /// </summary>
        /// <param name="metar">Raw METAR report</param>
        /// <returns><see cref="MetarData"/></returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="MetarNoDataException"/>
        public static MetarData Decode(string metar)
        {
            MetarNoDataException.ThrowIfNoData(metar);

            Match match_METAR = _reg_METAR.Match(metar);

            if (!match_METAR.Success)
            {
                throw new ArgumentException("Invalid METAR report", nameof(metar));
            }

            MetarData rs = new(metar);

            // report header
            if (match_METAR.Groups["header"].Value == "SPECI")
            {
                rs.Special = true;
            }

            // correction indicator
            if (match_METAR.Groups["correction"].Success)
            {
                rs.Correction = true;
            }

            // report airport
            rs.AirportICAO = match_METAR.Groups["station"].Value;

            // publish time
            rs.PublishTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, int.Parse(match_METAR.Groups["day"].Value), int.Parse(match_METAR.Groups["hour"].Value), int.Parse(match_METAR.Groups["minute"].Value), 0, DateTimeKind.Utc);

            // NIL indicator
            if (match_METAR.Groups["nil"].Success)
            {
                return rs;
            }

            // AUTO indicator
            if (match_METAR.Groups["auto"].Success)
            {
                rs.Auto = true;
            }

            // wind group
            rs.Wind.Raw = match_METAR.Groups["wind"].Value;
            if (match_METAR.Groups["wind"].Value != "/////")
            {
                if (match_METAR.Groups["wind_direction"].Value == "VRB")
                {
                    rs.Wind.IsVariable = true;
                    rs.Wind.VarDirectionStart = 0;
                    rs.Wind.VarDirectionEnd = 360;
                }
                else
                {
                    rs.Wind.Direction = int.Parse(match_METAR.Groups["wind_direction"].Value);
                }
                if (match_METAR.Groups["wind_speed"].Value == "P49")
                {
                    rs.Wind.Speed = 50;
                }
                else if (match_METAR.Groups["wind_speed"].Value == "P99")
                {
                    rs.Wind.Speed = 100;
                }
                else
                {
                    rs.Wind.Speed = int.Parse(match_METAR.Groups["wind_speed"].Value);
                }
                if (match_METAR.Groups["wind_guest"].Success)
                {
                    if (match_METAR.Groups["wind_guest"].Value == "GP49")
                    {
                        rs.Wind.Guest = 50;
                    }
                    else if (match_METAR.Groups["wind_guest"].Value == "GP99")
                    {
                        rs.Wind.Guest = 100;
                    }
                    else
                    {
                        rs.Wind.Guest = int.Parse(match_METAR.Groups["wind_guest"].Value[1..]);
                    }
                }
                if (match_METAR.Groups["wind_unit"].Value == "KT")
                {
                    rs.Wind.IsKnots = true;
                }
                if (match_METAR.Groups["wind_vardirection"].Success)
                {
                    rs.Wind.VarDirectionStart = int.Parse(match_METAR.Groups["wind_vardirection_start"].Value);
                    rs.Wind.VarDirectionEnd = int.Parse(match_METAR.Groups["wind_vardirection_end"].Value);
                }
            }

            // CAVOK
            if (match_METAR.Groups["condition"].Value == "CAVOK")
            {
                rs.CAVOK = true;
                goto TEMP;
            }

            // primary visbility group
            if (match_METAR.Groups["visbility"].Success && match_METAR.Groups["visbility"].Value != "////")
            {
                rs.Visibility.PrimaryVisibility = int.Parse(match_METAR.Groups["visbility_digital"].Value);
                if (match_METAR.Groups["visbility_unit"].Success)
                {
                    rs.Visibility.HorizontalVisibilityUnit = match_METAR.Groups["visbility_unit"].Value;
                }
            }
            else
            {
                rs.Visibility.PrimaryVisibility = -1;
            }

            // RVR group
            if (match_METAR.Groups["RVR"].Success)
            {
                rs.Visibility.RVR = [];
                foreach (Capture g in match_METAR.Groups["RVR"].Captures)
                {
                    Match match_RVR = _reg_RVR.Match(g.Value);
                    if (match_RVR.Success)
                    {
                        MetarVisibilityData.RVRData rvr = new();
                        if (match_RVR.Groups["prefix_lower"].Success)
                        {
                            rvr.LPM = match_RVR.Groups["prefix_lower"].Value == "P" ? MetarVisibilityData.RVRData.RVRPM.more_than_ : MetarVisibilityData.RVRData.RVRPM.less_than_;
                        }
                        if (match_RVR.Groups["RVR_lower"].Success)
                        {
                            rvr.LowerVisibility = int.Parse(match_RVR.Groups["RVR_lower"].Value);
                        }
                        if (match_RVR.Groups["prefix_upper"].Success)
                        {
                            rvr.UPM = match_RVR.Groups["prefix_upper"].Value == "P" ? MetarVisibilityData.RVRData.RVRPM.more_than_ : MetarVisibilityData.RVRData.RVRPM.less_than_;
                        }
                        rvr.UpperVisibility = int.Parse(match_RVR.Groups["RVR_upper"].Value);
                        if (match_RVR.Groups["trend"].Success)
                        {
                            rvr.Trends = match_RVR.Groups["trend"].Value switch
                            {
                                "U" => MetarVisibilityData.RVRData.RVRTrends.increasing,
                                "D" => MetarVisibilityData.RVRData.RVRTrends.decreasing,
                                _ => MetarVisibilityData.RVRData.RVRTrends.None,
                            };
                        }
                        rs.Visibility.RVR.Add(match_RVR.Groups["runway"].Value, rvr);
                    }
                }
            }

            // weather condition group
            if (match_METAR.Groups["weather"].Success && match_METAR.Groups["weather"].Value != " //")
            {
                foreach (Capture g in match_METAR.Groups["weather"].Captures)
                {
                    rs.ConditionRaw += g.Value;
                    rs.Condition += $"{MetarWeatherConditionDecoder.Parse(g.Value)}, ";
                }
                rs.ConditionRaw = rs.ConditionRaw.Trim();
            }

            // cloud group
            if (match_METAR.Groups["cloud"].Success && !_reg_no_data.IsMatch(match_METAR.Groups["cloud"].Value))
            {
                foreach (Capture g in match_METAR.Groups["cloud"].Captures)
                {
                    rs.Cloud.Add(MetarCloudData.Parse(g.Value.Trim()));
                }
            }

            // vertical visibility group
            if (match_METAR.Groups["vertical_visbility"].Success)
            {
                rs.Visibility.VerticalVisibility = int.Parse(match_METAR.Groups["vertical_visbility_digital"].Value);
            }

        // temperature group
        TEMP:
            rs.Temperature.Raw = $"{match_METAR.Groups["temperature"].Value}/{match_METAR.Groups["dewpoint"].Value}";
            if (match_METAR.Groups["temperature"].Value != "//")
            {
                rs.Temperature.Temperature = match_METAR.Groups["temperature"].Value.StartsWith('M') ? -1 * int.Parse(match_METAR.Groups["temperature"].Value[1..]) : int.Parse(match_METAR.Groups["temperature"].Value);
            }
            else
            {
                rs.Temperature.Temperature = int.MinValue;
            }
            if (match_METAR.Groups["dewpoint"].Value != "//")
            {
                rs.Temperature.Dewpoint = match_METAR.Groups["dewpoint"].Value.StartsWith('M') ? -1 * int.Parse(match_METAR.Groups["dewpoint"].Value[1..]) : int.Parse(match_METAR.Groups["dewpoint"].Value);
            }
            else
            {
                rs.Temperature.Dewpoint = int.MinValue;
            }

            // QNH group
            if (match_METAR.Groups["QNH_unit"].Value == "A")
            {
                rs.QNHUnit = MetarData.QNHUnits.inHg;
            }
            if (match_METAR.Groups["QNH_digital"].Value != "////")
            {
                rs.QNH = int.Parse(match_METAR.Groups["QNH_digital"].Value);
            }
            else
            {
                rs.QNH = -1;
            }

            string other_raw = match_METAR.Groups["other"].Value.Trim();

            // recent weather group
            MatchCollection mc_REs = _reg_REs.Matches(other_raw);
            foreach (Match m in mc_REs)
            {
                rs.REs += $"{m.Value} ";
            }
            rs.REs = rs.REs?.Trim() ?? string.Empty;
            other_raw = _reg_REs.Replace(other_raw, "").Trim();

            // wind shear group
            MatchCollection mc_WS = _reg_WS.Matches(other_raw);
            foreach (Match m in mc_WS)
            {
                rs.Windshear.Raw += $"{m.Value} ";
                if (m.Value == "WS ALL RWY")
                {
                    rs.Windshear.IsAllRwy = true;
                    break;
                }
                rs.Windshear.Rwys.Add(m.Groups["runway"].Value);
            }
            rs.Windshear.Raw = rs.Windshear.Raw?.Trim() ?? string.Empty;
            other_raw = _reg_WS.Replace(other_raw, "").Trim();

            // remarks group
            string[] rmk_split = other_raw.Split("RMK");
            other_raw = rmk_split[0];
            if (rmk_split.Length > 1)
            {
                rs.Remarks = $"RMK{rmk_split[1]}";
            }

            // trend group
            rs.Trend = other_raw.Trim();

            return rs;
        }

        #endregion Public Methods

        #region Private Methods

        [GeneratedRegex(@"(?<header>METAR|SPECI)(?<correction> COR)? (?<station>[A-Z0-9]{4}) (?<time>(?<day>\d{2})(?<hour>\d{2})(?<minute>\d{2}))Z(?<auto> AUTO)?(?<nil> NIL)? (?<wind>(?<wind_direction>\d{3}|VRB|///)(?<wind_speed>P?\d{2}|//)(?<wind_guest>GP?\d{2})?(?<wind_unit>KT|MPS)(?<wind_vardirection> (?<wind_vardirection_start>\d{3})V(?<wind_vardirection_end>\d{3}))?) (?<condition>CAVOK|(?<visbility>////|(?<visbility_digital>\d{4})|(?<visbility_digital>\d+)(?<visbility_unit>SM)|(?<visbility_digital>\d+)(?<visbility_unit>KM))(?<RVR> R[0-9LCR]+/\S+)*(?<weather> ([+-]|VC)?[A-Z/]{2,4}){0,3}?(?<cloud> /{6,}| SKC| NSC| (?<cloud_layer>[A-Z]{3})(?<cloud_ceiling>\d{3}|///)(?<cloud_type>\S+)?)*(?<vertical_visbility> VV(?<vertical_visbility_digital>\d{3}))?) (?<temperature>M?\d{2}|//)/(?<dewpoint>M?\d{2}|//) (?<QNH>(?<QNH_unit>[QA])(?<QNH_digital>\d{4}|////))(?<other>.*)")]
        private static partial Regex RegexMETAR();

        [GeneratedRegex(@" /+")]
        private static partial Regex RegexNoData();

        [GeneratedRegex(" (\\d{2})(\\d{2})(\\d{2})Z ")]
        private static partial Regex RegexPublishTime();

        [GeneratedRegex(@"RE\S+")]
        private static partial Regex RegexREs();

        [GeneratedRegex(@"R(?<runway>[0-9LCR]+)/(?<prefix_lower>[PM])?(?<RVR_lower>\d{4})?V?(?<prefix_upper>[PM])?(?<RVR_upper>\d{4})(?<trend>[UDN])?")]
        private static partial Regex RegexRVR();

        [GeneratedRegex(@"WS ALL RWY|WS (RW?Y?)(?<runway>[0-9LRC]+)")]
        private static partial Regex RegexWindShear();

        #endregion Private Methods
    }
}