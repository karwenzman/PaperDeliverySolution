using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File("LogFiles/apploggings.txt")
            .CreateLogger();

        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddLogging();

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
            Log.Logger.Error("Unexpected Exception: {error}", ex);
        }

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();

        base.OnExit(e);
    }
}
