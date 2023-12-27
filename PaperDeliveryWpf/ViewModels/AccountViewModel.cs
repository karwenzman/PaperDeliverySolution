using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.Repositories;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace PaperDeliveryWpf.ViewModels;

public partial class AccountViewModel : ViewModelBase, IAccountViewModel
{
    private string _displayName;
    private string _email;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonCommand))]
    [NotifyCanExecuteChangedFor(nameof(DiscardChangesButtonCommand))]
    private bool _currentUserHasChanged;

    [Required(ErrorMessage = "Enter your display name!")]
    public string DisplayName
    {
        get => _displayName;
        set
        {
            if (SetProperty(ref _displayName, value, true))
            {
                SaveChangesButtonCommand.NotifyCanExecuteChanged();
                DiscardChangesButtonCommand.NotifyCanExecuteChanged();
            }

            if (_displayName == _currentUser!.DisplayName)
            {
                CurrentUserHasChanged = false;
            }
            else
            {
                CurrentUserHasChanged = true;
            }
        }
    }

    [Required(ErrorMessage = "Enter your email address!")]
    public string Email
    {
        get => _email;
        set
        {
            if (SetProperty(ref _email, value, true))
            {
                SaveChangesButtonCommand.NotifyCanExecuteChanged();
                DiscardChangesButtonCommand.NotifyCanExecuteChanged();
            }

            if (_email == _currentUser!.Email)
            {
                CurrentUserHasChanged = false;
            }
            else
            {
                CurrentUserHasChanged = true;
            }
        }
    }

    [ObservableProperty]
    private string? _password; // can only be modified via a new view

    [ObservableProperty]
    private string? _role; // can only be modified by admin in accounts view

    [ObservableProperty]
    private bool _isActive; // can only be modified by admin in accounts view

    [ObservableProperty]
    private string _userName; // can not be modified by user

    [ObservableProperty]
    private string _lastLogin; // can not be modified by user

    [ObservableProperty]
    private string _lastModified; // can not be modified by user

    [ObservableProperty]
    private int _id; // can not be modified by user

    private readonly UserModel? _currentUser = new();

    private readonly ILogger<AccountViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public AccountViewModel(ILogger<AccountViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _currentUser = _userRepository.GetByUserName(GetUserName());

        if (!IsUserInRole("user"))
        {
            _logger.LogError("** Access denied on page {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"Access denied on page {nameof(AccountViewModel)} by {GetUserName()}!");
        }

        if (_currentUser == null)
        {
            _logger.LogError("** No valid user account is loaded while accessing {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"No valid user account is loaded while accessing class {nameof(AccountViewModel)} by {GetUserName()}!");
        }

        _logger.LogInformation("* Loading {class}", nameof(AccountViewModel));

        CurrentUserHasChanged = false;

        _id = _currentUser.Id;
        _isActive = _currentUser.IsActive;
        _userName = _currentUser.UserName;
        _displayName = _currentUser.DisplayName;
        _role = _currentUser.Role;
        _password = _currentUser.Password;
        _email = _currentUser.Email;
        _lastLogin = _currentUser.LastLogin;
        _lastModified = _currentUser.LastModified;
    }

    #region ***** RelayCommand *****
    [RelayCommand]
    public void CloseButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.HomeUserControl }));
    }

    [RelayCommand(CanExecute = nameof(CanResetPasswordButton))]
    public void ResetPasswordButton()
    {

    }
    public bool CanResetPasswordButton()
    {
        // Only true, if an admin opens AccountsView and selects an UserAccount.
        return false;
    }

    [RelayCommand(CanExecute = nameof(CanChangePasswordButton))]
    public void ChangePasswordButton()
    {

    }
    public bool CanChangePasswordButton()
    {
        // Only true, if an admin opens AccountsView and selects an UserAccount.
        return false;
    }

    [RelayCommand(CanExecute = nameof(CanSaveChangesButton))]
    public void SaveChangesButton()
    {
        if (_currentUser != null)
        {
            _currentUser.Email = Email;
            _currentUser.DisplayName = DisplayName;
            _userRepository.Update(_currentUser);
        }
        else
        {
            _logger.LogError("** No valid user account is loaded while accessing {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"No valid user account is loaded while accessing class {nameof(AccountViewModel)} by {GetUserName()}!");
        }

        CurrentUserHasChanged = false;
    }
    public bool CanSaveChangesButton()
    {
       return CurrentUserHasChanged && !HasErrors;
    }

    [RelayCommand(CanExecute = nameof(CanDiscardChangesButton))]
    public void DiscardChangesButton()
    {
        DisplayName = _currentUser!.DisplayName;
        Email = _currentUser.Email;
        CurrentUserHasChanged = false;
    }
    public bool CanDiscardChangesButton()
    {
        return CurrentUserHasChanged;
    }
    #endregion ***** End OF RelayCommand *****

}
