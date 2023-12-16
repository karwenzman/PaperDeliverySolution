using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellViewModel : ViewModelBase, IShellViewModel,
    IRecipient<ValueChangedMessage<ShellMessage>>,
    IRecipient<ValueChangedMessage<UserModel>>
{
    [ObservableProperty]
    private object? _currentView;

    [ObservableProperty]
    private string _applicationHomeDirectory = string.Empty;

    [ObservableProperty]
    private string _applicationName = string.Empty;

    [ObservableProperty]
    private string _applicationVersion = string.Empty;

    [ObservableProperty]
    private string _userEmail = string.Empty;

    [ObservableProperty]
    private string _userName = string.Empty;

    [ObservableProperty]
    private string _loginHeader = "Login";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginMenuItemCommand))]
    private ShellMessage? _shellMessage = new();

    [ObservableProperty]
    private bool _isActiveLoginMenuItem = true;

    private readonly ILogger<ShellViewModel> _logger;
    private readonly IOptions<ApplicationOptions> _options;
    private readonly IServiceProvider _serviceProvider;

    public ShellViewModel(ILogger<ShellViewModel> logger, IOptions<ApplicationOptions> options, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellViewModel));

        _options = options;
        ApplicationName = _options.Value.ApplicationName;
        ApplicationHomeDirectory = _options.Value.ApplicationHomeDirectory;
        ApplicationVersion = GetApplicationVersion();

        _serviceProvider = serviceProvider; // Check, if needed!

        CurrentView = App.AppHost!.Services.GetRequiredService<IStartViewModel>();

        StopCommand = new CommandBinding(ApplicationCommands.Stop, Stop, CanStop);

        WeakReferenceMessenger.Default.RegisterAll(this);
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
            ShellMessage = new ShellMessage { SetToActive = ActivateVisibility.LoginUserControl };
        }
        else if (LoginHeader == "Logout")
        {
            ShellMessage = new ShellMessage { SetToActive = ActivateVisibility.LoggedOutUserControl };
            UserName = string.Empty;
            UserEmail = string.Empty;
        }

        ManageUserControls(ShellMessage);
    }
    public bool CanLoginMenuItem()
    {
        return true;
    }
    #endregion ***** End OF RelayCommand *****

    private void ManageUserControls(ShellMessage? message)
    {
        if (message == null)
        {
            IsActiveLoginMenuItem = true;
            LoginHeader = $"Error in {nameof(ShellMessage)}";
            // TODO - Error Logging 
        }
        else
        {
            switch (message.SetToActive)
            {
                case ActivateVisibility.None:
                    IsActiveLoginMenuItem = false;
                    LoginHeader = "None"; // no effect, since IsActiveLoginMenuItem = false
                    break;
                case ActivateVisibility.LoginUserControl:
                    IsActiveLoginMenuItem = false;
                    LoginHeader = "LoginUserControl"; // no effect, since IsActiveLoginMenuItem = false
                    CurrentView = App.AppHost!.Services.GetRequiredService<ILoginViewModel>();
                    break;
                case ActivateVisibility.LoggedInUserControl:
                    IsActiveLoginMenuItem = true;
                    LoginHeader = "Logout";
                    CurrentView = App.AppHost!.Services.GetRequiredService<IHomeViewModel>();
                    break;
                case ActivateVisibility.LoggedOutUserControl:
                    IsActiveLoginMenuItem = true;
                    LoginHeader = "Login";
                    CurrentView = App.AppHost!.Services.GetRequiredService<IStartViewModel>();
                    break;
            }
        }
    }

    private void ManageUserInformation(UserModel? messge)
    {
        if (messge == null)
        {
            UserEmail = string.Empty;
            UserName = string.Empty;
        }
        else
        {
            UserEmail = messge.Email;
            UserName = messge.DisplayName;
        }
    }

    private string GetApplicationVersion()
    {
        // TODO - Work on Deployment
        //if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
        //{
        //    return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.
        //        CurrentVersion.ToString();
        //}
        //return "Not network deployed";

        // This is the assembly version only. See projet's properties. 
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        return $"{version!.Major}.{version!.Minor}.{version!.Build}.{version!.Revision}";
    }

    public void Receive(ValueChangedMessage<ShellMessage> message)
    {
        ManageUserControls(message.Value);
    }

    public void Receive(ValueChangedMessage<UserModel> message)
    {
        ManageUserInformation(message.Value);
    }
}
