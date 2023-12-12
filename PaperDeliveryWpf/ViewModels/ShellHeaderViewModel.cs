using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellHeaderViewModel : ViewModelBase, IShellHeaderViewModel
{
    // Constructor injection.
    private readonly ILogger<ShellHeaderViewModel> _logger;

    // Commands.
    public CommandBinding StopCommand { get; set; }


    public ShellHeaderViewModel(ILogger<ShellHeaderViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellHeaderViewModel));

        // Setting up the command bindings.
        StopCommand = new CommandBinding(ApplicationCommands.Stop, Stop, CanStop);
    }

    // Command bindings.
    private void Stop(object sender, ExecutedRoutedEventArgs e)
    {
        Application.Current.MainWindow.Close();
    }
    private void CanStop(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    // ShellView-Window's events (not for the UserControls!).
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

    // Relay commands.

}
