using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaperDeliveryWpf.ViewModels;
using PaperDeliveryWpf.Views;
using Serilog;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PaperDeliveryWpf
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger =new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging();
                    //services.AddOptions<ApplicationOptions>().Bind(context.Configuration.GetSection(nameof(ApplicationOptions)));

                    services.AddSingleton<ShellView>();
                    services.AddSingleton<IShellViewModel, ShellViewModel>();
                })
                .UseSerilog()
                .Build();
        }
    }

}
