using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows;

namespace PaperDeliveryWpf.ViewModels;

public partial class AccountManagerViewModel : ViewModelBase, IAccountManagerViewModel
{

    private string? _displayName;
    private string? _email;
    private string? _role;

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

            if (_displayName == CurrentUser!.DisplayName)
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

            if (_email == CurrentUser!.Email)
            {
                CurrentUserHasChanged = false;
            }
            else
            {
                CurrentUserHasChanged = true;
            }
        }
    }

    [Required(ErrorMessage = "Enter your user role!")]
    [AllowedValues(["guest", "user", "admin"], ErrorMessage = "Select a valid role (guest, user or admin)")]
    public string? Role
    {
        get => _role;
        set
        {
            if (SetProperty(ref _role, value, true))
            {
                SaveChangesButtonCommand.NotifyCanExecuteChanged();
                DiscardChangesButtonCommand.NotifyCanExecuteChanged();
            }

            if (_role == CurrentUser!.Role)
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
    private bool _isVisibleCloseAccountButton;

    [ObservableProperty]
    private bool _isVisibleAdminControl;

    [ObservableProperty]
    private bool _isEnabledAdminControl;

    [ObservableProperty]
    private ObservableCollection<UserModel> _userAccounts = [];

    [ObservableProperty]
    private UserModel? _selectedUserAccount;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonCommand))]
    [NotifyCanExecuteChangedFor(nameof(DiscardChangesButtonCommand))]
    private bool _currentUserHasChanged;

    [ObservableProperty]
    private UserModel? _currentUser;

    [ObservableProperty]
    private string? _password;

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



    private readonly ILogger<AccountManagerViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public AccountManagerViewModel(ILogger<AccountManagerViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;

        if (IsUserInRole("admin"))
        {
            IsEnabledAdminControl = IsUserInRole("admin");
            IsVisibleAdminControl = IsUserInRole("admin");
            IsVisibleCloseAccountButton = false;

            UserAccounts = _userRepository.GetAllRecords();

            _logger.LogInformation("* Loading {class}", nameof(AccountManagerViewModel));
        }
        else
        {
            _logger.LogError("** Access denied on page {class} by {name}!", nameof(AccountManagerViewModel), GetUserName());
            throw new ArgumentException($"Access denied on page {nameof(AccountManagerViewModel)} by {GetUserName()}!");
        }
    }

    partial void OnSelectedUserAccountChanged(UserModel? value)
    {
        CurrentUser = SelectedUserAccount;
        CheckUserAccountAndAssignToLocalProperties();
    }

    #region ***** RelayCommand *****
    [RelayCommand]
    public void AddButton()
    {
        Debug.WriteLine($"Passed AddButton: {CurrentUser.UserName}");
        CheckUserAccountAndAssignToLocalProperties();
    }

    [RelayCommand]
    public void DeleteButton()
    {
        Debug.WriteLine($"Passed DeleteButton: {CurrentUser.UserName}");
        CheckUserAccountAndAssignToLocalProperties();

    }

    [RelayCommand]
    public void FindButton()
    {
        Debug.WriteLine($"Passed FindButton: {CurrentUser.UserName}");
        CheckUserAccountAndAssignToLocalProperties();
    }

    [RelayCommand]
    public static void CloseButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = LoadViewModel.HomeUserControl }));
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
        if (CurrentUser != null)
        {
            string message;
            string caption = nameof(SaveChangesButton);

            // Providing just the accessable members. Change might be necessary. 
            CurrentUser.Email = Email;
            CurrentUser.DisplayName = DisplayName!;
            if (_userRepository.UpdateAccount(CurrentUser))
            {
                message = "Update successful.\nThe changes have been saved to your account.";
                CurrentUser = _userRepository.GetByUserName(GetUserName());
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
            _logger.LogError("** No valid user account is loaded while accessing {class} by {name}!", nameof(AccountManagerViewModel), GetUserName());
            throw new ArgumentException($"No valid user account is loaded while accessing class {nameof(AccountManagerViewModel)} by {GetUserName()}!");
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
        DisplayName = CurrentUser!.DisplayName;
        Email = CurrentUser.Email;
        Role = CurrentUser.Role;
        CurrentUserHasChanged = false;
    }
    public bool CanDiscardChangesButton()
    {
        return CurrentUserHasChanged;
    }
    #endregion ***** End Of RelayCommand *****

    private void CheckUserAccount()
    {
        if (CurrentUser == null)
        {
            _logger.LogError("** No valid user account is loaded while accessing {class} by {name}!", nameof(AccountManagerViewModel), GetUserName());
            throw new ArgumentException($"No valid user account is loaded while accessing class {nameof(AccountManagerViewModel)} by {GetUserName()}!");
        }
    }

    private void CheckUserAccountAndAssignToLocalProperties()
    {
        CheckUserAccount();

        Id = CurrentUser!.Id;
        IsActive = CurrentUser!.IsActive;
        UserName = CurrentUser!.UserName;
        DisplayName = CurrentUser!.DisplayName;
        Role = CurrentUser!.Role;
        Password = CurrentUser!.Password;
        Email = CurrentUser!.Email;
        LastLogin = CurrentUser!.LastLogin;
        LastModified = CurrentUser!.LastModified;
    }
}
