using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryWpf.Repositories;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    // Constructor injection.
    private readonly ILogger<LoginViewModel> _logger;
    private readonly IServiceProvider _serviceProvider;

    // Private fields to store the loaded services.
    private readonly IUserRepository _userRepository;
    private ShellMessage _message = new();
    private UserModel? _user = new();

    // Properties using CommunityToolkit.
    [ObservableProperty]
    private string _showSomething;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginButtonCommand))]
    private string _uiLogin = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginButtonCommand))]
    private string _uiPassword = string.Empty;

    public LoginViewModel(ILogger<LoginViewModel> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoginViewModel));

        _serviceProvider = serviceProvider;
        _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();

        ShowSomething = "Hallo Welt!";
    }

    #region ***** RelayCommand *****
    [RelayCommand(CanExecute = nameof(CanLoginButton))]
    public void LoginButton()
    {
        // TODO - Error handling.
        _user = _userRepository.Login(UiLogin, UiPassword);
        ShowSomething = _user!.DisplayName;

        _message = new ShellMessage
        {
            SetToActive = ActivateVisibility.LoggedInUserControl,
        };

        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(_user));
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(_message));

        _logger.LogInformation("** User {user} has logged in.", _user.Email);

        // TODO - How to close this UserControl?
    }
    public bool CanLoginButton()
    {
        // TODO - How to enable this, if a character is entered into TextBox?
        bool output = true;
        if (string.IsNullOrWhiteSpace(UiLogin) && string.IsNullOrWhiteSpace(UiPassword))
        {
            output = false;
        }

        //bool output = false;
        //if (UiLogin.Length > 0 && UiPassword.Length > 0)
        //{
        //    output = true;
        //}

        return output;
    }

    [RelayCommand(CanExecute = nameof(CanCancelButton))]
    public void CancelButton()
    {
        ShowSomething = "User has canceled the login.";

        _message = new ShellMessage
        {
            SetToActive = ActivateVisibility.LoggedOutUserControl,
        };
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(_message));

        // TODO - How to close this UserControl?
    }
    public bool CanCancelButton()
    {
        return true;
    }
    #endregion ***** End Of RelayCommand *****
}
