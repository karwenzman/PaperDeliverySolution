using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Messages;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoggedInViewModel : ViewModelBase, ILoggedInViewModel, IRecipient<ValueChangedMessage<ShellMessage>>
{
    private readonly ILogger<LoggedInViewModel> _logger;

    [ObservableProperty]
    private bool _isActiveUserControl;

    public LoggedInViewModel(ILogger<LoggedInViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoggedInViewModel));

        WeakReferenceMessenger.Default.Register(this);

        IsActiveUserControl = false;
    }

    public void Receive(ValueChangedMessage<ShellMessage> message)
    {
        if (message.Value.DisplayLoggedIn)
        {
            IsActiveUserControl = true;
        }
        else
        {
            IsActiveUserControl = false;
        }
    }
}
