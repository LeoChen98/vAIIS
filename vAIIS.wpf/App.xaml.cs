using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace vAIIS.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    #region Public Constructors

    public App()
    {
    }

    #endregion Public Constructors

    #region Public Properties

    public static new App Current => (App)Application.Current;

    #endregion Public Properties
}