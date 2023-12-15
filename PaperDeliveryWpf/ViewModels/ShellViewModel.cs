using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Enums;
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
    private string _loginHeader = "Login";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginMenuItemCommand))]
    private ShellMessage _shellMessage = new();

    [ObservableProperty]
    private bool _isActiveLoginMenuItem = true;

    [ObservableProperty]
    private bool _isActiveLoginUserControl = false;

    [ObservableProperty]
    private bool _isActiveLoggedInUserControl = false;

    [ObservableProperty]
    private bool _isActiveLoggedOutUserControl = true;

    public ShellViewModel(ILogger<ShellViewModel> logger, IOptions<ApplicationOptions> options, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellViewModel));

        _options = options;
        ApplicationName = _options.Value.ApplicationName;
        ApplicationHomeDirectory = _options.Value.ApplicationHomeDirectory;

        _serviceProvider = serviceProvider;
        CurrentView = _serviceProvider.GetRequiredService<ILoggedOutViewModel>();

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
        //CurrentView = _serviceProvider.GetRequiredService<ILoggedInViewModel>();
        //CurrentView = _serviceProvider.GetRequiredService<ILoginViewModel>();
        //CurrentView = _serviceProvider.GetRequiredService<ILoggedOutViewModel>();

        if (LoginHeader == "Login")
        {
            ShellMessage.SetToActive = ActivateVisibility.LoginUserControl;
        }
        else if (LoginHeader == "Logout")
        {
            ShellMessage.SetToActive = ActivateVisibility.LoggedOutUserControl;
        }

        ManageUserControls();
    }
    public bool CanLoginMenuItem()
    {
        return true;
    }
    #endregion ***** End OF RelayCommand *****

    private void ManageUserControls()
    {
        switch (ShellMessage.SetToActive)
        {
            case ActivateVisibility.None:
                IsActiveLoginUserControl = false;
                IsActiveLoggedInUserControl = false;
                IsActiveLoggedOutUserControl = false;
                IsActiveLoginMenuItem = false;
                LoginHeader = "None"; // no effect, since IsActiveLoginMenuItem = false
                break;
            case ActivateVisibility.LoginUserControl:
                IsActiveLoginUserControl = true;
                IsActiveLoggedInUserControl = false;
                IsActiveLoggedOutUserControl = false;
                IsActiveLoginMenuItem = false;
                LoginHeader = "LoginUserControl"; // no effect, since IsActiveLoginMenuItem = false
                break;
            case ActivateVisibility.LoggedInUserControl:
                IsActiveLoginUserControl = false;
                IsActiveLoggedInUserControl = true;
                IsActiveLoggedOutUserControl = false;
                IsActiveLoginMenuItem = true;
                LoginHeader = "Logout";
                break;
            case ActivateVisibility.LoggedOutUserControl:
                IsActiveLoginUserControl = false;
                IsActiveLoggedInUserControl = false;
                IsActiveLoggedOutUserControl = true;
                IsActiveLoginMenuItem = true;
                LoginHeader = "Login";
                break;
        }
    }


    public void Receive(ValueChangedMessage<ShellMessage> message)
    {
        Debug.WriteLine($"Message received by {nameof(ShellViewModel)}.");

        ShellMessage = message.Value;

        ManageUserControls();
    }

}
