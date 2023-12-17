using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.UserControls;

namespace PaperDeliveryWpf.ViewModels;

public partial class LogoutViewModel : ViewModelBase, ILogoutViewModel
{
    private readonly IShellViewModel _shellViewModel;

    private readonly ILogger<LogoutUserControl> _logger;

    public LogoutViewModel(ILogger<LogoutUserControl> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LogoutViewModel));

        _shellViewModel = App.AppHost!.Services.GetRequiredService<IShellViewModel>();
    }

    [RelayCommand]
    public void LogoutButton()
    {
        UserModel userAccount = _shellViewModel.UserAccount;

        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(new UserModel()));
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.StartUserControl }));
        _logger.LogInformation("** User {user} has logged out.", userAccount.Login);
    }

    [RelayCommand]
    public void CancelButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.HomeUserControl }));
    }

}
