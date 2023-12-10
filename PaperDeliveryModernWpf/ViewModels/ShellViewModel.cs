using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.ProjectOptions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryModernWpf.ViewModels;

public partial class ShellViewModel : ViewModelBase, IShellViewModel
{
    private readonly ILogger<ShellViewModel> _logger;
    private readonly IOptions<ApplicationOptions> _applicationOptions;

    public CommandBinding StopCommand { get; set; }

    [ObservableProperty]
    private string? _applicationHomeDirectory;

    [ObservableProperty]
    //[NotifyDataErrorInfo]
    //[Required]
    private string? _developerName;

    [ObservableProperty]
    private string? _applicationName;

    public ShellViewModel(ILogger<ShellViewModel> logger, IOptions<ApplicationOptions> applicationOptions)
    {
        _logger = logger;
        _applicationOptions = applicationOptions;

        _logger.LogInformation("* Loading: {class}", nameof(ShellViewModel));

        ApplicationHomeDirectory = _applicationOptions.Value.ApplicationHomeDirectory;
        ApplicationName = _applicationOptions.Value.ApplicationName;

        // Setting up the command bindings.
        StopCommand = new CommandBinding(ApplicationCommands.Stop, Stop, CanStop);

        DeveloperName = "karwenzman";
        DeveloperName = "Thorsten";
    }

    #region ***** Testing only *****
    /// <summary>
    /// For testing the CommunityToolkit.MVVM
    /// </summary>
    /// <param name="value"></param>
    partial void OnDeveloperNameChanged(string? value)
    {
        Debug.WriteLine($"The property {nameof(DeveloperName)} is changed to the new value: {value}");
    }
    /// <summary>
    /// For testing the CommunityToolkit.MVVM
    /// </summary>
    /// <param name="value"></param>
    partial void OnDeveloperNameChanging(string? value)
    {
        Debug.WriteLine($"The property {nameof(DeveloperName)} is going to be changed to the new value: {value}");
    }
    #endregion

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
