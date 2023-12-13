using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Messages;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoggedInViewModel : ViewModelBase, ILoggedInViewModel
{
    private readonly ILogger<LoggedInViewModel> _logger;

    [ObservableProperty]
    private bool _isActiveUserControl;

    private ShellMessage _message = new();

    public LoggedInViewModel(ILogger<LoggedInViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoggedInViewModel));


        IsActiveUserControl = true;

        _message = new ShellMessage
        {
            DisplayLoggedIn = false,
            DisplayLoggedOut = true,
            DisplayLogin = false,
        };

        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(_message));
    }
}
