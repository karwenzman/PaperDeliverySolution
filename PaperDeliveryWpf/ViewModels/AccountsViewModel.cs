using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using PaperDeliveryWpf.Repositories;
using System.Collections.ObjectModel;
using PaperDeliveryLibrary.Models;

namespace PaperDeliveryWpf.ViewModels;

public partial class AccountsViewModel : ViewModelBase, IAccountsViewModel
{

    [ObservableProperty]
    private bool _isVisibleCloseAccountButton;

    [ObservableProperty]
    private bool _isVisibleAdminControl;

    [ObservableProperty]
    private bool _isEnabledAdminControl;

    [ObservableProperty]
    private ObservableCollection<UserModel> _userAccounts = [];

    private readonly ILogger<AccountsViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public AccountsViewModel(ILogger<AccountsViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;

        if (IsUserInRole("admin"))
        {
            IsEnabledAdminControl = IsUserInRole("admin");
            IsVisibleAdminControl = IsUserInRole("admin");
            IsVisibleCloseAccountButton = false;

            UserAccounts = _userRepository.GetAllRecords();

            _logger.LogInformation("* Loading {class}", nameof(AccountsViewModel));
        }
        else
        {
            _logger.LogError("** Access denied on page {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"Access denied on page {nameof(AccountViewModel)} by {GetUserName()}!");
        }
    }


    #region ***** RelayCommand *****
    [RelayCommand]
    public static void CloseAccountsButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.HomeUserControl }));
    }
    #endregion ***** End Of RelayCommand *****
}
