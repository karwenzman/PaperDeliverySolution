using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.Repositories;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    private readonly IUserRepository _userRepository;
    private ShellMessage? _message = new();
    private UserModel? _user = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginButtonCommand))]
    private string _uiLogin = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginButtonCommand))]
    private string _uiPassword = string.Empty;

    private readonly ILogger<LoginViewModel> _logger;
    private readonly IServiceProvider _serviceProvider;

    public LoginViewModel(ILogger<LoginViewModel> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoginViewModel));

        _serviceProvider = serviceProvider;
        _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
    }

    #region ***** RelayCommand *****
    [RelayCommand(CanExecute = nameof(CanLoginButton))]
    public void LoginButton()
    {
        // TODO - Error handling.
        _user = _userRepository.Login(UiLogin, UiPassword);
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(_user));

        _message = new ShellMessage
        {
            SetToActive = ActivateVisibility.HomeUserControl,
        };

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
        _message = new ShellMessage
        {
            SetToActive = ActivateVisibility.StartUserControl,
        };
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(_message));

        _user = new();
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(_user));

        // TODO - How to close this UserControl?
    }
    public bool CanCancelButton()
    {
        return true;
    }
    #endregion ***** End Of RelayCommand *****
}
