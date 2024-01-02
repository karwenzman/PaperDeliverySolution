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

public partial class AccountViewModel : ViewModelBase, IAccountViewModel, IRecipient<ValueChangedMessage<AccountMessage>>
{
    private UserModel? _currentAccount;

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

            if (_displayName == _currentAccount!.DisplayName)
            {
                CurrentAccountHasChanged = false;
            }
            else
            {
                CurrentAccountHasChanged = true;
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

            if (_email == _currentAccount!.Email)
            {
                CurrentAccountHasChanged = false;
            }
            else
            {
                CurrentAccountHasChanged = true;
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

            if (_role == _currentAccount!.Role)
            {
                CurrentAccountHasChanged = false;
            }
            else
            {
                CurrentAccountHasChanged = true;
            }
        }
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonCommand))]
    [NotifyCanExecuteChangedFor(nameof(DiscardChangesButtonCommand))]
    private bool _currentAccountHasChanged;

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

    #region ***** UI controls *****
    [ObservableProperty] private bool _isVisibleCloseButton;
    [ObservableProperty] private bool _isVisibleResetPasswordButton;
    [ObservableProperty] private bool _isVisibleChangePasswordButton;
    [ObservableProperty] private bool _isVisibleSaveChangesButton;
    [ObservableProperty] private bool _isVisibleDiscardChangesButton;

    [ObservableProperty] private bool _isEnabledUserName;
    [ObservableProperty] private bool _isEnabledDisplayName;
    [ObservableProperty] private bool _isEnabledEmail;
    [ObservableProperty] private bool _isEnabledIsActive;
    [ObservableProperty] private bool _isEnabledUserId;
    [ObservableProperty] private bool _isEnabledRole;
    [ObservableProperty] private bool _isEnabledLastModified;
    [ObservableProperty] private bool _isEnabledLastLogin;

    #endregion ***** End of UI controls *****

    private readonly ILogger<AccountViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public AccountViewModel(ILogger<AccountViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _currentAccount = _userRepository.GetByUserName(GetUserName());

        if (IsUserInRole("user"))
        {
            AssignCurrentUserToLocalProperties();
            SetViewToDefault();

            CurrentAccountHasChanged = false;

            _logger.LogInformation("* Loading {class}", nameof(AccountViewModel));
        }
        else
        {
            _logger.LogError("** Access denied on page {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"Access denied on page {nameof(AccountViewModel)} by {GetUserName()}!");
        }
    }

    #region ***** RelayCommand *****
    [RelayCommand]
    public void CloseButton()
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
        if (_currentAccount != null)
        {
            string message;
            string caption = nameof(SaveChangesButton);

            // Providing just the accessable members. Change might be necessary. 
            _currentAccount.Email = Email;
            _currentAccount.DisplayName = DisplayName!;
            if (_userRepository.UpdateAccount(_currentAccount))
            {
                message = "Update successful.\nThe changes have been saved to your account.";
                _currentAccount = _userRepository.GetByUserName(GetUserName());
                AssignCurrentUserToLocalProperties();
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

        CurrentAccountHasChanged = false;
    }
    public bool CanSaveChangesButton()
    {
        return CurrentAccountHasChanged && !HasErrors;
    }

    [RelayCommand(CanExecute = nameof(CanDiscardChangesButton))]
    public void DiscardChangesButton()
    {
        DisplayName = _currentAccount!.DisplayName;
        Email = _currentAccount.Email;
        Role = _currentAccount.Role;
        CurrentAccountHasChanged = false;
    }
    public bool CanDiscardChangesButton()
    {
        return CurrentAccountHasChanged;
    }
    #endregion ***** End OF RelayCommand *****

    /// <summary>
    /// If the current user is null the error is logged and an exception is thrown.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    private void CheckCurrentUserIsNull()
    {
        if (_currentAccount == null)
        {
            _logger.LogError("** No valid user account is loaded while accessing {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"No valid user account is loaded while accessing class {nameof(AccountViewModel)} by {GetUserName()}!");
        }
    }

    /// <summary>
    /// Assigns to values of the current user to the oberservable properties used in the view.
    /// </summary>
    private void AssignCurrentUserToLocalProperties()
    {
        Id = _currentAccount!.Id;
        IsActive = _currentAccount!.IsActive;
        UserName = _currentAccount!.UserName;
        DisplayName = _currentAccount!.DisplayName;
        Role = _currentAccount!.Role;
        Email = _currentAccount!.Email;
        LastLogin = _currentAccount!.LastLogin;
        LastModified = _currentAccount!.LastModified;
    }

    /// <summary>
    /// This setting is used, if an user opens <see cref="AccountViewModel"/>.
    /// </summary>
    private void SetViewToDefault()
    {
        IsVisibleCloseButton = true;
        IsVisibleResetPasswordButton = false;
        IsVisibleChangePasswordButton = true;
        IsVisibleSaveChangesButton = true;
        IsVisibleDiscardChangesButton = true;

        IsEnabledUserName = false;
        IsEnabledDisplayName = true;
        IsEnabledEmail = true;
        IsEnabledIsActive = false;
        IsEnabledUserId = false;
        IsEnabledRole = false;
        IsEnabledLastModified = false;
        IsEnabledLastLogin = false;
    }

    /// <summary>
    /// This setting is used, if an admin opens <see cref="AccountManagerViewModel"/>
    /// and selects an item from the data grid.
    /// </summary>
    private void SetViewToAccountManagerSelectedItem()
    {
        IsVisibleCloseButton = false;
        IsVisibleResetPasswordButton = true;
        IsVisibleChangePasswordButton = false;
        IsVisibleSaveChangesButton = true;
        IsVisibleDiscardChangesButton = true;

        IsEnabledUserName = false;
        IsEnabledDisplayName = true;
        IsEnabledEmail = true;
        IsEnabledIsActive = true;
        IsEnabledUserId = false;
        IsEnabledRole = true;
        IsEnabledLastModified = false;
        IsEnabledLastLogin = false;
    }

    /// <summary>
    /// This setting is used, if an admin opens <see cref="AccountManagerViewModel"/>
    /// and clicks on the add button.
    /// </summary>
    private void SetViewToAccountManagerAddItem()
    {
        IsVisibleCloseButton = false;
        IsVisibleResetPasswordButton = false;
        IsVisibleChangePasswordButton = false;
        IsVisibleSaveChangesButton = true;
        IsVisibleDiscardChangesButton = true;

        IsEnabledUserName = true;
        IsEnabledDisplayName = true;
        IsEnabledEmail = true;
        IsEnabledIsActive = false;
        IsEnabledUserId = false;
        IsEnabledRole = true;
        IsEnabledLastModified = false;
        IsEnabledLastLogin = false;
    }

    /// <summary>
    /// This method is executed, if this instances receives a messages from <see cref="AccountManagerViewModel"/>.
    /// </summary>
    /// <param name="message"></param>
    public void Receive(ValueChangedMessage<AccountMessage> message)
    {
        _currentAccount = message.Value.SelectedUser;
        CheckCurrentUserIsNull();
        AssignCurrentUserToLocalProperties();

        switch (message.Value.SetAccountUserControl)
        {
            case SetAccountUserControl.Default:
                // This is never called by AccountsViewModel, but set all the controls anyway.
                SetViewToDefault();
                break;
            case SetAccountUserControl.AccountManagerAddItem:
                // This is called, if in AccountsUserControl the AddButton event was raised.
                SetViewToAccountManagerAddItem();
                break;
            case SetAccountUserControl.AccountManagerSelectedItem:
                // This is called, if in AccountsUserControl the SelectionChanged event was raised.
                SetViewToAccountManagerSelectedItem();
                break;
            default:
                // Can never take place.
                // kind of error message or so ???
                break;
        }
    }
}
