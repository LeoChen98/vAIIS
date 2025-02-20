using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using vAIIS.Wpf.Extension;
#if !DEBUG
using Serilog.Events;
#endif

namespace vAIIS.Wpf
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            using var host = CreateHostBuilder().Build();

            App app = new App
            {
                MainWindow = host.Services.GetRequiredService<View.MainWindow>()
            };
            app.MainWindow.Show();
            app.Run();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(container =>
                {
                    ConfigureWindows(container);
                    ConfigureViewModels(container);
                    ConfigureFoundation(container);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.File(
                            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\vAIIS\\log\\log.log",
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 7)
#if !DEBUG
                        .WriteTo.Sentry(o =>
                        {
                            o.Dsn = "#SENTRY-DSN#";
                            o.MinimumBreadcrumbLevel = LogEventLevel.Information;
                            o.MinimumEventLevel = LogEventLevel.Error;
                        })
#endif
                        .CreateLogger();
                    logging.AddSerilog();
                })
                .ConfigureAppConfiguration(config =>
                {
                    config.AddWritableJsonFile(
                        $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\vAIIS\\config\\plugins.json",
                        true); // plugin list.
                });
        }

        private static void ConfigureFoundation(IServiceCollection container)
        {
            container.AddSingleton<Foundation.PluginManager>();
        }

        /// <summary>
        /// Configure Windows
        /// </summary>
        /// <param name="container">IOC service collection</param>
        private static void ConfigureWindows(IServiceCollection container)
        {
            // MainWindow
            container.AddSingleton<View.MainWindow>(sp => new View.MainWindow
                { DataContext = sp.GetRequiredService<ViewModel.MainViewModel>() });
        }

        /// <summary>
        /// Configure ViewModels    
        /// </summary>
        /// <param name="container">IOC service collection</param>
        private static void ConfigureViewModels(IServiceCollection container)
        {
            // MainViewModel    
            container.AddSingleton<ViewModel.MainViewModel>();
        }
    }
}