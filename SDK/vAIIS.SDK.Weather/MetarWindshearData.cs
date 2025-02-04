using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vAIIS.SDK.Weather;


/// <summary>
/// Windshear data
/// </summary>
public class MetarWindshearData
{
    #region Public Fields

    /// <summary>
    /// A flag indicates whether the windshear affects all runways, default is false.
    /// </summary>
    public bool IsAllRwy = false;

    /// <summary>
    /// Raw report of windshear data
    /// </summary>
    public string Raw = string.Empty;

    /// <summary>
    /// A list of affected runways, default is empty. If <see cref="IsAllRwy"/> is true, this list is empty.
    /// </summary>
    public List<string> Rwys;

    #endregion Public Fields

    #region Public Constructors

    public MetarWindshearData()
    {
        Rwys = [];
    }

    #endregion Public Constructors
}
