using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoggedOutViewModel : ViewModelBase, ILoggedOutViewModel, IRecipient<ValueChangedMessage<ShellMessage>>
{
    private readonly ILogger<LoggedOutViewModel> _logger;

    [ObservableProperty]
    private bool _isActiveUserControl;

    public LoggedOutViewModel(ILogger<LoggedOutViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoggedOutViewModel));

        WeakReferenceMessenger.Default.Register(this);

        IsActiveUserControl = false;
    }

    public void Receive(ValueChangedMessage<ShellMessage> message)
    {
        if (message.Value.DisplayLoggedOut)
        {
            IsActiveUserControl = true;
        }
        else
        {
            IsActiveUserControl = false;
        }
    }
}
