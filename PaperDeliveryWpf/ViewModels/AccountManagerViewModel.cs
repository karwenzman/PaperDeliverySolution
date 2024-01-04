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
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PaperDeliveryWpf.ViewModels;

public partial class AccountManagerViewModel : ViewModelBase, IAccountManagerViewModel, IRecipient<ValueChangedMessage<AccountManagerMessage>>
{
    [ObservableProperty]
    private object? _accountViewModel = new();

    [ObservableProperty]
    private ObservableCollection<UserModel> _accounts = [];

    [ObservableProperty]
    private UserModel? _currentAccount;
    partial void OnCurrentAccountChanged(UserModel? value)
    {
        // TODO (Issue #9) - After receiving a message from AccountViewModel value == Null ???
        if (value != null)
        {
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<AccountMessage>(new AccountMessage { Account = value, SetAccountUserControl = SetAccountUserControl.AccountManagerSelectedItem }));
            Debug.WriteLine($"Passed OnCurrentAccountChanged: {value.DisplayName}");
        }
        else
        {
            Debug.WriteLine($"Passed OnCurrentAccountChanged. CurrentAccount = null");

        }
    }

    private readonly ILogger<AccountManagerViewModel> _logger;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// This 'ViewModel' handles the display and edit functions for a all accounts of type <see cref="UserModel"/>.
    /// <para></para>
    /// Just calling this constructor is not enough:
    /// <br></br>- the logic starts with sending a message of type implementing <see cref="IAccountManagerMessage"/>
    /// <br></br>- the sender needs to provide the <see cref="UserModel"/> and the <see cref="SetAccountUserControl"/>
    /// <br></br>- all these information are handled in <see cref="Receive(ValueChangedMessage{AccountManagerMessage})"/>
    /// <para></para>
    /// What the constructor does:
    /// <br></br>- checking the user role; if the current user of the app does not have at least 'admin' role an error message is thrown
    /// <br></br>- logging the access; the current user of the app will be logged with its 'UserName'
    /// <br></br>- registering to the <see cref="WeakReferenceMessenger"/>
    /// <br></br>- loading the child 'ViewModel' of tpye implementing <see cref="IAccountViewModel"/>
    /// </summary>
    /// <param name="logger">Access to the logging system.</param>
    /// <param name="userRepository">Access to the repository system.</param>
    /// <exception cref="ArgumentException"></exception>
    public AccountManagerViewModel(ILogger<AccountManagerViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;

        if (IsUserInRole("admin"))
        {
            _logger.LogInformation("* Loading {class}", nameof(AccountManagerViewModel));
            WeakReferenceMessenger.Default.RegisterAll(this);

            AccountViewModel = App.AppHost!.Services.GetRequiredService<IAccountViewModel>();
        }
        else
        {
            _logger.LogError("** Access denied on page {class} by {name}!", nameof(AccountManagerViewModel), GetUserName());
            throw new ArgumentException($"Access denied on page {nameof(AccountManagerViewModel)} by {GetUserName()}!");
        }
    }

    /// <summary>
    /// This method is executed, if this instance receives a message from <see cref="AccountViewModel"/>.
    /// </summary>
    /// <param name="message"></param>
    public void Receive(ValueChangedMessage<AccountManagerMessage> message)
    {
        Debug.WriteLine($"Passed Receive: {nameof(AccountManagerMessage)}");

        if (message.Value.Account == null)
        {
            _logger.LogError("** No valid user account is received while accessing page {class} by {name}!", nameof(AccountManagerViewModel), GetUserName());
            throw new ArgumentException($"No valid user account is received while accessing page {nameof(AccountManagerViewModel)} by {GetUserName()}!");
        }
        else
        {
            Accounts = _userRepository.GetAllRecords();
            CurrentAccount = _userRepository.GetByUserName(message.Value.Account.UserName);
        }
    }

    #region ***** RelayCommand *****
    [RelayCommand]
    public void AddButton()
    {
        Debug.WriteLine($"Passed AddButton: {CurrentAccount!.UserName}");
    }

    [RelayCommand]
    public void DeleteButton()
    {
        Debug.WriteLine($"Passed DeleteButton: {CurrentAccount!.UserName}");

    }

    [RelayCommand]
    public void FindButton()
    {
        Debug.WriteLine($"Passed FindButton: {CurrentAccount!.UserName}");
    }

    [RelayCommand]
    public static void CloseButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = LoadViewModel.HomeUserControl }));
    }
    #endregion ***** End Of RelayCommand *****
}
