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

    [ObservableProperty]
    private int _currentAccountIndex = 0;

    private int _oldAccountIndex;

    private readonly ILogger<AccountManagerViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public AccountManagerViewModel(ILogger<AccountManagerViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;

        if (IsUserInRole("admin"))
        {
            Accounts = _userRepository.GetAllRecords();
            CurrentAccount = Accounts[CurrentAccountIndex];
            AccountViewModel = App.AppHost!.Services.GetRequiredService<IAccountViewModel>();

            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<IAccountMessage>(new AccountMessage { Account = CurrentAccount, SetAccountUserControl = SetAccountUserControl.AccountManagerSelectedItem }));
            WeakReferenceMessenger.Default.RegisterAll(this);

            //WeakReferenceMessenger.Default.Register<AccountManagerMessage>(this, (r, m) =>
            //{
            //    // code is never accessed ???
            //    Accounts = _userRepository.GetAllRecords();
            //    CurrentAccount = _userRepository.GetByUserName(m.UpdatedAccount!.UserName);
            //});

            _logger.LogInformation("* Loading {class}", nameof(AccountManagerViewModel));
        }
        else
        {
            _logger.LogError("** Access denied on page {class} by {name}!", nameof(AccountManagerViewModel), GetUserName());
            throw new ArgumentException($"Access denied on page {nameof(AccountManagerViewModel)} by {GetUserName()}!");
        }
    }

    partial void OnCurrentAccountChanged(UserModel? value)
    {
        // TODO (Issue #9) - After receiving a message from AccountViewModel value == Null ???
        if (value != null)
        {
            // This _oldAccountIndex is used in the Receive() method to set the value and update the data grid.
            _oldAccountIndex = CurrentAccountIndex;
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<AccountMessage>(new AccountMessage { Account = value, SetAccountUserControl = SetAccountUserControl.AccountManagerSelectedItem }));
            Debug.WriteLine($"Passed OnCurrentAccountChanged: {value.DisplayName}");
        }
        else
        {
            Debug.WriteLine($"Passed OnCurrentAccountChanged. CurrentAccount = null");

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

    /// <summary>
    /// This method is executed, if this instance receives a message from <see cref="AccountViewModel"/>.
    /// </summary>
    /// <param name="message"></param>
    public void Receive(ValueChangedMessage<AccountManagerMessage> message)
    {
        Debug.WriteLine($"Passed Receive: {CurrentAccount!.UserName}");

        Accounts = _userRepository.GetAllRecords();
        CurrentAccountIndex = _oldAccountIndex;
        //CurrentAccount = _userRepository.GetByUserName(message.Value.UpdatedAccount!.UserName);
    }

}
