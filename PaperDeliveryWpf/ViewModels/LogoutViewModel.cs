using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;

namespace PaperDeliveryWpf.ViewModels;

public partial class LogoutViewModel : ViewModelBase, ILogoutViewModel
{
    private readonly IShellViewModel _shellViewModel;

    private readonly ILogger<LogoutViewModel> _logger;

    public LogoutViewModel(ILogger<LogoutViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LogoutViewModel));

        _shellViewModel = App.AppHost!.Services.GetRequiredService<IShellViewModel>();
    }

    [RelayCommand]
    public void LogoutButton()
    {
        _logger.LogInformation("** User {user} has logged out.", _shellViewModel.CurrentUser!.UserName);

        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(new UserModel()));
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.StartUserControl }));
    }

    [RelayCommand]
    public void CancelButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.HomeUserControl }));
    }
}
