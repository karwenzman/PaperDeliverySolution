using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;
using PaperDeliveryWpf.ViewModels;
using PaperDeliveryWpf.Views;
using Serilog;
using System.Windows;

namespace PaperDeliveryWpf
{
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

            // Dependency injection.
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging();
                    services.AddOptions<ApplicationOptions>().Bind(context.Configuration.GetSection(nameof(ApplicationOptions)));

                    services.AddSingleton<IUserModel, UserModel>();

                    services.AddSingleton<ShellView>();
                    services.AddSingleton<IShellViewModel, ShellViewModel>();
                    services.AddTransient<IShellBodyViewModel, ShellBodyViewModel>();
                    services.AddTransient<IShellFooterViewModel, ShellFooterViewModel>();
                    services.AddTransient<IShellHeaderViewModel, ShellHeaderViewModel>();
                    services.AddTransient<ILoginViewModel, LoginViewModel>();
                    services.AddTransient<IHomeLoggedInViewModel, HomeLoggedInViewModel>();
                    services.AddTransient<IHomeLoggedOutViewModel, HomeLoggedOutViewModel>();
                })
                .UseSerilog()
                .Build();

            Log.Logger.Information("***** {namespace} *****", nameof(PaperDeliveryWpf));

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            try
            {
                // TODO - Where is the app, if the exception is thrown? It does not stop the app. => Issue #2
                //throw new Exception();
                var mainWindow = AppHost.Services.GetRequiredService<ShellView>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal("Unexpected exception, while starting the application: {error}", ex);
            }

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();

            base.OnExit(e);
        }
    }
}
