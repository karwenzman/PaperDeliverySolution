using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Messages;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoggedOutViewModel : ViewModelBase, ILoggedOutViewModel
{
    private readonly ILogger<LoggedOutViewModel> _logger;

    [ObservableProperty]
    private bool _isActiveUserControl;

    private readonly ShellMessage _message;

    public LoggedOutViewModel(ILogger<LoggedOutViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoggedOutViewModel));

        IsActiveUserControl = true;

        _message = new ShellMessage
        {
            DisplayLoggedIn = true,
            DisplayLoggedOut = false,
            DisplayLogin = false,
        };

        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(_message));
    }
}
