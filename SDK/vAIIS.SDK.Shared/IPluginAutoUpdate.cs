﻿namespace vAIIS.SDK.Shared;

/// <summary>
/// Interface for plugin auto update.
/// </summary>
public interface IPluginAutoUpdate
{
    #region Public Methods

    /// <summary>
    /// Check if there is an update available.
    /// </summary>
    /// <returns><see cref="CheckUpdateResult"/></returns>
    public Task<CheckUpdateResult> CheckUpdateAsync();

    /// <summary>
    /// Check if there is an update available.
    /// </summary>
    /// <param name="state">Anything needs to be sent to the method.</param>
    /// <returns><see cref="CheckUpdateResult"/></returns>
    public Task<CheckUpdateResult> CheckUpdateAsync(object state);

    /// <summary>
    /// Do anything required after updating.
    /// </summary>
    /// <param name="oldVersion">Old version of the plugin. Can be <see langword="null"/>.</param>
    /// <remarks>
    /// note:
    /// <list type="bullet">
    /// <item>This method will be called ( in NEW version dll ) after the plugin is updated.</item>
    /// <item>Can be used to update the plugin settings or do any other required tasks.</item>
    /// <item>If nothing to do, <see langword="return"/> directly.</item>
    /// <item>
    /// The host will try to fill the <paramref name="oldVersion"/>, but if no data, fill with
    ///    <see langword="null"/>. <see cref="NullReferenceException"/> should be expected if this
    ///    <paramref name="oldVersion"/> is used in the method.
    /// </item>
    /// </list>
    /// </remarks>
    /// <returns></returns>
    public Task FinishUpdateAsync(string? oldVersion);

    #endregion Public Methods
}