using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    private UserModel? _currentUser = new();

    private string _uiUserName = string.Empty;
    private string _uiPassword = string.Empty;

    [Required(ErrorMessage = "Enter your user name!")]
    public string UiUserName
    {
        get => _uiUserName;
        set
        {
            if (SetProperty(ref _uiUserName, value, true))
            {
                LoginButtonCommand.NotifyCanExecuteChanged();
            }
        }
    }

    [Required(ErrorMessage = "Enter your password!")]
    [MinLength(3, ErrorMessage = "You need to enter minimum 3 charaters.")]
    public string UiPassword
    {
        get => _uiPassword;
        set
        {
            if (SetProperty(ref _uiPassword, value, true))
            {
                LoginButtonCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private readonly ILogger<LoginViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public LoginViewModel(ILogger<LoginViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;

        _logger.LogInformation("* Loading {class}", nameof(LoginViewModel));
    }

    #region ***** RelayCommand *****
    [RelayCommand(CanExecute = nameof(CanLoginButton))]
    public void LoginButton()
    {
        bool validUser = _userRepository.Authenticate(new NetworkCredential(UiUserName, UiPassword));

        if (validUser)
        {
            _currentUser = _userRepository.GetByUserName(UiUserName);
            ArgumentNullException.ThrowIfNull(_currentUser);

            CreateThreadPrincipal(_currentUser.UserName, GetUserRoles(_currentUser.Role), "access database");

            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.HomeUserControl }));
            _logger.LogInformation("** User {user} has logged in.", _currentUser.UserName);
        }
        else
        {
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.ErrorUserControl }));
        }
    }
    public bool CanLoginButton()
    {
        bool output = true;

        if (string.IsNullOrWhiteSpace(UiUserName))
        {
            output = false;
        }
        if (string.IsNullOrWhiteSpace(UiPassword))
        {
            output = false;
        }
        if (HasErrors)
        {
            output = false;
        }

        return output;
    }


    [RelayCommand]
    public void CancelButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.StartUserControl }));
    }
    #endregion ***** End Of RelayCommand *****
}
