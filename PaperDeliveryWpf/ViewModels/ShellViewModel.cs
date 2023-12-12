using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.ProjectOptions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellViewModel : ViewModelBase, IShellViewModel
{
    private readonly ILogger<ShellViewModel> _logger;
    private readonly IOptions<ApplicationOptions> _options;

    //public CommandBinding StopCommand { get; set; }

    [ObservableProperty]
    private string? _applicationHomeDirectory;

    [ObservableProperty]
    private string? _applicationName;



    public ShellViewModel(ILogger<ShellViewModel> logger, IOptions<ApplicationOptions> options)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellViewModel));

        _options = options;
        ApplicationName = _options.Value.ApplicationName;
        ApplicationHomeDirectory = _options.Value.ApplicationHomeDirectory;


        // Setting up the command bindings.
        //StopCommand = new CommandBinding(ApplicationCommands.Stop, Stop, CanStop);
    }

    //private void Stop(object sender, ExecutedRoutedEventArgs e)
    //{
    //    Application.Current.MainWindow.Close();
    //}
    //private void CanStop(object sender, CanExecuteRoutedEventArgs e)
    //{
    //    e.CanExecute = true;
    //}

    //public void ShellView_Closing(object? sender, CancelEventArgs e)
    //{
    //    MessageBoxResult messageBoxResult = MessageBox.Show(
    //        "Soll das Fenster geschlossen werden?",
    //        $"{nameof(ShellView_Closing)}",
    //        MessageBoxButton.YesNo,
    //        MessageBoxImage.Question,
    //        MessageBoxResult.No);

    //    if (messageBoxResult == MessageBoxResult.No)
    //    {
    //        e.Cancel = true;
    //    }
    //}
}
