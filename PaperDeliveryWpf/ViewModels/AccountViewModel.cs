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
using System.Diagnostics;
using System.Windows;

namespace PaperDeliveryWpf.ViewModels;

public partial class AccountViewModel : ViewModelBase, IAccountViewModel,
    IRecipient<ValueChangedMessage<AccountMessage>>
{
    private UserModel? _currentAccount;
    private SetAccountUserControl _currentUiSetting;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonCommand))]
    [NotifyCanExecuteChangedFor(nameof(DiscardChangesButtonCommand))]
    private bool _currentAccountHasChanged;

    #region ***** UI properties *****
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
            SetCurrentAccountHasChanged();
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
            SetCurrentAccountHasChanged();
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
            SetCurrentAccountHasChanged();
        }
    }

    [ObservableProperty]
    private bool _isActive;
    partial void OnIsActiveChanged(bool value)
    {
        SetCurrentAccountHasChanged();
    }

    [ObservableProperty]
    private string? _userName;

    [ObservableProperty]
    private string? _lastLogin;

    [ObservableProperty]
    private string? _lastModified;

    [ObservableProperty]
    private int _id;
    #endregion ***** UI properties *****

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

    /// <summary>
    /// This 'ViewModel' handles the display and edit functions for a single account of type <see cref="UserModel"/>.
    /// <para></para>
    /// Just calling this constructor is not enough:
    /// <br></br>- the logic starts with sending a message of type implementing <see cref="IAccountMessage"/>
    /// <br></br>- the sender needs to provide the <see cref="UserModel"/> and the <see cref="SetAccountUserControl"/>
    /// <br></br>- all these information are handled in <see cref="Receive(ValueChangedMessage{AccountMessage})"/>
    /// <para></para>
    /// What the constructor does:
    /// <br></br>- checking the user role; if the current user of the app does not have at least 'user' role an error message is thrown
    /// <br></br>- logging the access; the current user of the app will be logged with its 'UserName'
    /// <br></br>- registering to the <see cref="WeakReferenceMessenger"/>
    /// </summary>
    /// <param name="logger">Access to the logging system.</param>
    /// <param name="userRepository">Access to the repository system.</param>
    /// <exception cref="ArgumentException"></exception>
    public AccountViewModel(ILogger<AccountViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;

        if (IsUserInRole("user"))
        {
            _logger.LogInformation("* Loading {class}", nameof(AccountViewModel));
            WeakReferenceMessenger.Default.RegisterAll(this);
        }
        else
        {
            _logger.LogError("** Access denied on page {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"Access denied on page {nameof(AccountViewModel)} by {GetUserName()}!");
        }
    }

    /// <summary>
    /// This method is executed, if this instance receives a message of type <see cref="AccountMessage"/>.
    /// <para></para>
    /// All ViewModels calling <see cref="AccountViewModel"/> must use <see cref="CommunityToolkit.Mvvm.Messaging"/>.
    /// <br></br>In this message the account to display needs to be provided.
    /// <br></br>In this message the configuration of UI needs to be provided.
    /// <code>WeakReferenceMessenger.Default.Send();</code>
    /// </summary>
    /// <param name="message"></param>
    public void Receive(ValueChangedMessage<AccountMessage> message)
    {
        Debug.WriteLine($"Passed Receive: {nameof(AccountMessage)}");

        if (message.Value.Account == null)
        {
            _logger.LogError("** No valid user account is received while accessing page {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"No valid user account is received while accessing page {nameof(AccountViewModel)} by {GetUserName()}!");
        }
        else
        {
            _currentAccount = message.Value.Account;
            _currentUiSetting = message.Value.SetAccountUserControl;

            AssignAccountToLocalProperties(_currentAccount);

            switch (_currentUiSetting)
            {
                case SetAccountUserControl.Default:
                    SetViewToDefault();
                    break;
                case SetAccountUserControl.AccountManagerAddItem:
                    SetViewToAccountManagerAddItem();
                    break;
                case SetAccountUserControl.AccountManagerSelectedItem:
                    SetViewToAccountManagerSelectedItem();
                    break;
            }
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
        Debug.WriteLine($"Passed SaveChangesButton: {nameof(AccountMessage)}");

        string message;
        string caption = nameof(SaveChangesButton);

        string? oldEmail = _currentAccount!.Email;
        string? oldDisplayName = _currentAccount!.DisplayName;
        string? oldRole = _currentAccount!.Role;
        bool oldIsActive = _currentAccount!.IsActive;

        _currentAccount!.Email = Email;
        _currentAccount!.DisplayName = DisplayName!;
        _currentAccount!.Role = Role!;
        _currentAccount!.IsActive = IsActive!;

        if (_userRepository.UpdateAccount(_currentAccount!))
        {
            message = "Update successful.\n\nThe changes have been saved.";

            switch (_currentUiSetting)
            {
                case SetAccountUserControl.Default:
                    WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = LoadViewModel.AccountUserControl }));
                    break;
                case SetAccountUserControl.AccountManagerAddItem:
                    Debug.WriteLine($"Passed SaveChangesButton when trying to add a new account");
                    break;
                case SetAccountUserControl.AccountManagerSelectedItem:
                    if (_currentAccount.UserName == GetUserName())
                    {
                        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = LoadViewModel.AccountManagerUserControl }));
                    }
                    else
                    {
                        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<AccountManagerMessage>(new AccountManagerMessage { Account = _currentAccount }));
                    }
                    break;
            }

        }
        else
        {
            message = "Update failed.\n\nThe changes have not been saved. Restoring the old values.";
            Email = oldEmail;
            DisplayName = oldDisplayName;
            Role = oldRole;
            IsActive = oldIsActive;
            CurrentAccountHasChanged = false;
        }

        ShowMessageBox(message, caption);
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
        IsActive = _currentAccount.IsActive;
        CurrentAccountHasChanged = false;
    }
    public bool CanDiscardChangesButton()
    {
        return CurrentAccountHasChanged;
    }
    #endregion ***** End OF RelayCommand *****


    private void SetCurrentAccountHasChanged()
    {
        if (Email == _currentAccount!.Email &&
            Role == _currentAccount!.Role &&
            IsActive == _currentAccount!.IsActive &&
            DisplayName == _currentAccount!.DisplayName)
        {
            CurrentAccountHasChanged = false;
        }
        else
        {
            CurrentAccountHasChanged = true;
        }
    }

    /// <summary>
    /// Assigns to values of the current user to the oberservable properties used in the view.
    /// </summary>
    private void AssignAccountToLocalProperties(UserModel account)
    {
        Id = account.Id;
        IsActive = account.IsActive;
        UserName = account.UserName;
        DisplayName = account.DisplayName;
        Role = account.Role;
        Email = account.Email;
        LastLogin = account.LastLogin;
        LastModified = account.LastModified;
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
    /// This method is calling <see cref="MessageBox.Show()"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="caption"></param>
    /// <returns></returns>
    private static MessageBoxResult ShowMessageBox(string message, string caption)
    {
        // TODO (Issue #10) - MessageBoxes should not be handled by the ViewModel.

        MessageBoxResult messageBoxResult = MessageBox.Show(
            messageBoxText: message,
            caption: caption,
            MessageBoxButton.OK,
            MessageBoxImage.Information,
            MessageBoxResult.No);

        return messageBoxResult;
    }

}
