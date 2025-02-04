using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vAIIS.Wpf
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();

            App.Current.MainWindow = host.Services.GetRequiredService<View.MainWindow>();
            App.Current.MainWindow.Show();
            App.Current.Run();

        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(container =>
                {
                    ConfigureWindows(container);
                    ConfigureViewModels(container);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.File($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\vAIIS\\log\\log.log", rollingInterval: RollingInterval.Day)
                        .CreateLogger();
                    logging.AddSerilog();
                });
        }

        /// <summary>
        /// Configure Windows
        /// </summary>
        /// <param name="services">IOC service collection</param>
        private static void ConfigureWindows(IServiceCollection container)
        {
            // MainWindow
            container.AddSingleton<View.MainWindow>(sp => new View.MainWindow { DataContext = sp.GetRequiredService<ViewModel.MainViewModel>() });

        }
        /// <summary>
        /// Configure ViewModels    
        /// </summary>
        /// <param name="services">IOC service collection</param>
        private static void ConfigureViewModels(IServiceCollection container)
        {
            // MainViewModel    
            container.AddSingleton<ViewModel.MainViewModel>();
        }
    }
}
