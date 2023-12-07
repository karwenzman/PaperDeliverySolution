using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaperDeliveryLibrary.ProjectOptions;
using PaperDeliveryModernWpf.ViewModels;
using PaperDeliveryModernWpf.Views;
using Serilog;
using System.Windows;

namespace PaperDeliveryModernWpf;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        // Enables Serilog to read configuration from appsettings.json and environment variables.
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        // All the configuration for Serilog is done in appsettings.json.
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddLogging();
                services.AddOptions<ApplicationOptions>().Bind(context.Configuration.GetSection(nameof(ApplicationOptions)));

                services.AddSingleton<ShellView>();
                services.AddSingleton<IShellViewModel, ShellViewModel>();
            })
            .UseSerilog()
            .Build();

        Log.Logger.Information("***** {namespace} *****", nameof(PaperDeliveryModernWpf));
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        try
        {
            var mainWindow = AppHost.Services.GetRequiredService<ShellView>();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal("Unexpected Exception: {error}", ex);
        }

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();

        base.OnExit(e);
    }
}
