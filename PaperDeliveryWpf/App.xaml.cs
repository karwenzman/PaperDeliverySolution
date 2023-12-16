using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaperDeliveryLibrary.ProjectOptions;
using PaperDeliveryWpf.Repositories;
using PaperDeliveryWpf.ViewModels;
using PaperDeliveryWpf.Views;
using Serilog;
using System.Windows;

namespace PaperDeliveryWpf
{
    public partial class App : Application
    {
        /// <summary>
        /// This global property is providing access to the Dependency-Injection-System.
        /// <code>
        /// var viewModel = (ILoginViewModel)App.AppHost!.Services.GetService(typeof(ILoginViewModel))!;
        /// </code>
        /// Just replace the <see cref="Type"/> to the one needed in your situation.
        /// This code is used in the code behind files of the <see cref="UserControls"/>,
        /// since these classes do need a parameterless constructor.
        /// </summary>
        public static IHost? AppHost { get; private set; }

        public App()
        {
            // TODO - Check, if userSecrets does work with this setting.
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

                    services.AddSingleton<IUserRepository, UserRepositoryFake>();

                    services.AddSingleton<ShellView>();
                    services.AddSingleton<IShellViewModel, ShellViewModel>();
                    services.AddTransient<ILoginViewModel, LoginViewModel>();
                    services.AddTransient<IHomeViewModel, HomeViewModel>();
                    services.AddTransient<IStartViewModel, StartViewModel>();
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
