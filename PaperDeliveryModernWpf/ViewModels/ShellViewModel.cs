using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryModernWpf.ViewModels;

public partial class ShellViewModel : ObservableObject, IShellViewModel
{
    private readonly ILogger<ShellViewModel> _logger;

    public CommandBinding StopCommand { get; set; }

    [ObservableProperty]
    private string? _developerName;

    public ShellViewModel(ILogger<ShellViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading: {class}", nameof(ShellViewModel));

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
