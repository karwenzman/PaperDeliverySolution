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
using System.Windows;

namespace PaperDeliveryWpf.ViewModels;

public partial class AccountViewModel : ViewModelBase, IAccountViewModel
{
    private UserModel? _currentUser;

    private string? _displayName;
    private string? _email;

    [Required(ErrorMessage = "Enter your display name!")]
    public string? DisplayName
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
    public string? Email
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
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonCommand))]
    [NotifyCanExecuteChangedFor(nameof(DiscardChangesButtonCommand))]
    private bool _currentUserHasChanged;

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private string? _role;

    [ObservableProperty]
    private bool _isActive;

    [ObservableProperty]
    private string? _userName;

    [ObservableProperty]
    private string? _lastLogin;

    [ObservableProperty]
    private string? _lastModified;

    [ObservableProperty]
    private int _id;

    private readonly ILogger<AccountViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public AccountViewModel(ILogger<AccountViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _currentUser = _userRepository.GetByUserName(GetUserName());

        CheckUserRole("user");
        CheckUserAccountAndAssignToLocalProperties();

        CurrentUserHasChanged = false;

        _logger.LogInformation("* Loading {class}", nameof(AccountViewModel));
    }

    #region ***** RelayCommand *****
    [RelayCommand]
    public void CloseAccountButton()
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
            string message;
            string caption = nameof(SaveChangesButton);

            // Providing just the accessable members. Change might be necessary. 
            _currentUser.Email = Email;
            _currentUser.DisplayName = DisplayName!;
            if (_userRepository.UpdateAccount(_currentUser))
            {
                message = "Update successful.\nThe changes have been saved to your account.";
                _currentUser = _userRepository.GetByUserName(GetUserName());
                CheckUserAccountAndAssignToLocalProperties();
            }
            else
            {
                message = "Update failed.\nThe changes have not been saved to your account.";
            }

            // TODO - MessageBoxes should not be handled by the ViewModel.
            MessageBoxResult messageBoxResult = MessageBox.Show(
                messageBoxText: message,
                caption: caption,
                MessageBoxButton.OK,
                MessageBoxImage.Information,
                MessageBoxResult.No);
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

    private void CheckUserRole(string userRole)
    {
        if (!IsUserInRole(userRole))
        {
            _logger.LogError("** Access denied on page {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"Access denied on page {nameof(AccountViewModel)} by {GetUserName()}!");
        }
    }

    private void CheckUserAccount()
    {
        if (_currentUser == null)
        {
            _logger.LogError("** No valid user account is loaded while accessing {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"No valid user account is loaded while accessing class {nameof(AccountViewModel)} by {GetUserName()}!");
        }
    }

    private void CheckUserAccountAndAssignToLocalProperties()
    {
        CheckUserAccount();

        Id = _currentUser!.Id;
        IsActive = _currentUser!.IsActive;
        UserName = _currentUser!.UserName;
        DisplayName = _currentUser!.DisplayName;
        Role = _currentUser!.Role;
        Password = _currentUser!.Password;
        Email = _currentUser!.Email;
        LastLogin = _currentUser!.LastLogin;
        LastModified = _currentUser!.LastModified;
    }
}
