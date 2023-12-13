using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.ProjectOptions;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellViewModel : ViewModelBase, IShellViewModel, IRecipient<ValueChangedMessage<ShellMessage>>
{
    private readonly ILogger<ShellViewModel> _logger;
    private readonly IOptions<ApplicationOptions> _options;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private object? _currentView;

    [ObservableProperty]
    private string? _applicationHomeDirectory;

    [ObservableProperty]
    private string? _applicationName;

    [ObservableProperty]
    private string _loginHeader;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginMenuItemCommand))]
    private ShellMessage _shellMessage = new();

    [ObservableProperty]
    private bool _isActiveLoginMenuItem;


    public ShellViewModel(ILogger<ShellViewModel> logger, IOptions<ApplicationOptions> options, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellViewModel));

        _options = options;
        ApplicationName = _options.Value.ApplicationName;
        ApplicationHomeDirectory = _options.Value.ApplicationHomeDirectory;

        _serviceProvider = serviceProvider;
        CurrentView = _serviceProvider.GetRequiredService<ILoggedOutViewModel>();

        LoginHeader = "Login";
        IsActiveLoginMenuItem = true;

        StopCommand = new CommandBinding(ApplicationCommands.Stop, Stop, CanStop);

        WeakReferenceMessenger.Default.Register(this);
    }

    #region ***** Event *****
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
    #endregion ***** End Of Event *****

    #region ***** CommandBinding *****
    public CommandBinding StopCommand { get; set; }
    private void Stop(object sender, ExecutedRoutedEventArgs e)
    {
        Application.Current.MainWindow.Close();
    }
    private void CanStop(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    #endregion ***** End OF CommandBinding *****

    #region ***** RelayCommand *****
    [RelayCommand(CanExecute = nameof(CanLoginMenuItem))]
    public void LoginMenuItem()
    {
        if (LoginHeader == "Login")
        {
            // Clicking "Login".
            IsActiveLoginMenuItem = true; // for testing only
            //IsActiveLoginMenuItem = false;
            LoginHeader = "Should be collapsed";
            CurrentView = _serviceProvider.GetRequiredService<ILoginViewModel>();
        }
        else if (LoginHeader == "Logout")
        {
            // Clicking "Logout".
            IsActiveLoginMenuItem = true;
            LoginHeader = "Login";
            CurrentView = _serviceProvider.GetRequiredService<ILoggedOutViewModel>();
        }
        else if (LoginHeader == "Should be collapsed")
        {
            // Clicking "OK" or "Cancel" in the "LoginUserControl".
            IsActiveLoginMenuItem = true;
            LoginHeader = "Logout";
            CurrentView = _serviceProvider.GetRequiredService<ILoggedInViewModel>();
        }
    }
    public bool CanLoginMenuItem()
    {
        return true;
    }
    #endregion ***** End OF RelayCommand *****

    public void Receive(ValueChangedMessage<ShellMessage> message)
    {
        Debug.WriteLine($"Message received by {nameof(ShellViewModel)}.");
        //if (message.Value.DisplayLogin)
        //{
        //    IsActiveLoginMenuItem = true; // for testing only
        //    //IsActiveLoginMenuItem = false;
        //    LoginHeader = "Should be collapsed";
        //    CurrentView = _serviceProvider.GetRequiredService<ILoginViewModel>();
        //}
        //else if (message.Value.DisplayLoggedIn)
        //{
        //    IsActiveLoginMenuItem = true;
        //    LoginHeader = "Logout";
        //    CurrentView = _serviceProvider.GetRequiredService<ILoggedInViewModel>();
        //}
        //else if (message.Value.DisplayLoggedOut)
        //{
        //    IsActiveLoginMenuItem = true;
        //    LoginHeader = "Login";
        //    CurrentView = _serviceProvider.GetRequiredService<ILoggedOutViewModel>();
        //}
    }

}
