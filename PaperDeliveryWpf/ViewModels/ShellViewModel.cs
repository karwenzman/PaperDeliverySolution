using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.ProjectOptions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellViewModel : ViewModelBase, IShellViewModel
{
    private readonly ILogger<ShellViewModel> _logger;
    private readonly IOptions<ApplicationOptions> _options;

    public CommandBinding StopCommand { get; set; }

    [ObservableProperty]
    private string? _applicationHomeDirectory;

    [ObservableProperty]
    private string? _applicationName;

    [ObservableProperty]
    private bool _needToLogin;

    [ObservableProperty]
    private string _loginHeader;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginMenuItemCommand))]
    private ShellMessage _shellMessage = new();

    [ObservableProperty]
    private bool _isActiveUserControl;

    [ObservableProperty]
    private bool _isActiveMenuItem;

    public ShellViewModel(ILogger<ShellViewModel> logger, IOptions<ApplicationOptions> options)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellViewModel));

        _options = options;
        ApplicationName = _options.Value.ApplicationName;
        ApplicationHomeDirectory = _options.Value.ApplicationHomeDirectory;

        ShellMessage = new ShellMessage
        {
            DisplayLoggedIn = false,
            DisplayLoggedOut = true,
            DisplayLogin = false,
        };
        //ShellMessage.DisplayLoggedOut = true;
        NeedToLogin = true;
        LoginHeader = "Login Start";
        IsActiveMenuItem = true;
        IsActiveUserControl = true;

        StopCommand = new CommandBinding(ApplicationCommands.Stop, Stop, CanStop);
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

    [RelayCommand(CanExecute = nameof(CanLoginMenuItem))]
    public void LoginMenuItem()
    {
        if (ShellMessage.DisplayLoggedIn)
        {
            // User clicked "Logut".
            LoginHeader = "Login";
            IsActiveMenuItem = true;
            ShellMessage.DisplayLoggedOut = true;
            ShellMessage.DisplayLoggedIn = false;
            ShellMessage.DisplayLogin = false;
        }
        else if (ShellMessage.DisplayLoggedOut)
        {
            // User clicked "Login".
            LoginHeader = "Should be collapsed";
            IsActiveMenuItem = false;
            IsActiveUserControl = false;
            ShellMessage.DisplayLoggedOut = false;
            ShellMessage.DisplayLoggedIn = false;
            ShellMessage.DisplayLogin = true;
        }
        else if (ShellMessage.DisplayLogin)
        {
            // User finished "LoginView".
            LoginHeader = "Logout";
            IsActiveMenuItem = true;
            ShellMessage.DisplayLoggedOut = false;
            ShellMessage.DisplayLoggedIn = true;
            ShellMessage.DisplayLogin = false;
        }
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(ShellMessage));
    }
    public bool CanLoginMenuItem()
    {
        return true;
    }

    public void Receive(ValueChangedMessage<ShellMessage> message)
    {
        throw new NotImplementedException();
    }

}
