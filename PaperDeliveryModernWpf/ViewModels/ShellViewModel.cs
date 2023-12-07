using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.ProjectOptions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryModernWpf.ViewModels;

public partial class ShellViewModel : ObservableObject, IShellViewModel
{
    private readonly ILogger<ShellViewModel> _logger;
    private readonly IOptions<ApplicationOptions> _applicationOpptions;

    public CommandBinding StopCommand { get; set; }

    [ObservableProperty]
    private string? _applicationHomeDirectory;
    [ObservableProperty]
    private string? _developerName;
    [ObservableProperty]
    private string? _applicationName;

    public ShellViewModel(ILogger<ShellViewModel> logger, IOptions<ApplicationOptions> applicationOptions)
    {
        _logger = logger;
        _applicationOpptions = applicationOptions;

        _logger.LogInformation("* Loading: {class}", nameof(ShellViewModel));

        ApplicationHomeDirectory = _applicationOpptions.Value.ApplicationHomeDirectory;
        ApplicationName = _applicationOpptions.Value.ApplicationName;

        // Setting up the command bindings.
        StopCommand = new CommandBinding(ApplicationCommands.Stop, Stop, CanStop);

        DeveloperName = "karwenzman";
    }


    private void Stop(object sender, ExecutedRoutedEventArgs e)
    {
        Application.Current.MainWindow.Close();
    }
    private void CanStop(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    public void ShellView_Closing(object? sender, CancelEventArgs e)
    {
        MessageBoxResult messageBoxResult = MessageBox.Show(
            "Soll das Fenster geschlossen werden?",
            $"{nameof(ShellView_Closing)}",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question,
            MessageBoxResult.No);

        if (messageBoxResult == MessageBoxResult.No)
        {
            e.Cancel = true;
        }
    }
}
