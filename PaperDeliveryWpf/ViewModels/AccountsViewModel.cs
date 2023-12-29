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

namespace PaperDeliveryWpf.ViewModels;

public partial class AccountsViewModel : ViewModelBase, IAccountsViewModel
{



    private readonly ILogger<AccountsViewModel> _logger;

    public AccountsViewModel(ILogger<AccountsViewModel> logger)
    {
        _logger = logger;


        _logger.LogInformation("* Loading {class}", nameof(AccountsViewModel));
    }


    #region ***** RelayCommand *****
    [RelayCommand]
    public void CloseAccountsButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.HomeUserControl }));
    }
    #endregion ***** End Of RelayCommand *****
}
