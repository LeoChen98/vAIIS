namespace vAIIS.SDK.Shared;

/// <summary>
/// Result of checking update.
/// </summary>
public sealed class CheckUpdateResult
{
    #region Public Constructors

    /// <summary>
    /// Constructor for <see cref="CheckUpdateResult"/>.
    /// </summary>
    /// <param name="hasUpdate">Flag to indicate if there is an update available.</param>
    /// <param name="updateUrl">URL to download the update.</param>
    /// <param name="updateVersion">New version of the update.</param>
    /// <exception cref="ArgumentException"/>
    /// <remarks>
    /// note: The update url should be a link to a zip file containing the plugin dll and any other
    ///       files required for the plugin to work.
    /// </remarks>
    public CheckUpdateResult(bool hasUpdate, string? updateUrl = null, string? updateVersion = null)
    {
        if (hasUpdate && (string.IsNullOrWhiteSpace(updateUrl) || string.IsNullOrWhiteSpace(updateVersion)))
        {
            throw new ArgumentException("Update URL and Update Version must be provided if there is an update available.");
        }

        HasUpdate = hasUpdate;
        UpdateUrl = updateUrl;
        UpdateVersionString = updateVersion;
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// Flag to indicate if there is an update available.
    /// </summary>
    public bool HasUpdate { get; init; }

    /// <summary>
    /// URL to download the update.
    /// </summary>
    /// <remarks>
    /// note: The update url should be a link to a zip file containing the plugin dll and any other
    ///       files required for the plugin to work.
    /// </remarks>
    public string? UpdateUrl { get; init; }

    /// <summary>
    /// New version of the update.
    /// </summary>
    public string? UpdateVersionString { get; init; }

    /// <summary>
    /// New version of the update.
    /// </summary>
    public Version? UpdateVersion => Version.TryParse(UpdateVersionString, out var version) ? version : null;

    #endregion Public Properties
}