using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Security;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    private UserModel? _currentUser = new();

    private string? _userName;

    [Required(ErrorMessage = "Enter your user name!")]
    public string? UserName
    {
        get => _userName;
        set
        {
            if (SetProperty(ref _userName, value, true))
            {
                LoginButtonCommand.NotifyCanExecuteChanged();
            }
        }
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginButtonCommand))]
    private SecureString? _password;

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
        bool validUser = _userRepository.Authenticate(new NetworkCredential(UserName, Password));

        if (validUser)
        {
            _currentUser = _userRepository.GetByUserName(UserName);
            ArgumentNullException.ThrowIfNull(_currentUser);

            CreateThreadPrincipal(_currentUser.UserName, GetUserRoles(_currentUser.Role), "access database");
            _userRepository.UpdateLastLogin(_currentUser);

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

        if (string.IsNullOrWhiteSpace(UserName))
        {
            output = false;
        }
        if (Password == null || Password.Length == 0)
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
    public static void CancelButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.StartUserControl }));
    }
    #endregion ***** End Of RelayCommand *****
}
