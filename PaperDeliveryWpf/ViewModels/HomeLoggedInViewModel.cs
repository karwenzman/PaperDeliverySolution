using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Messages;

namespace PaperDeliveryWpf.ViewModels;

public partial class HomeLoggedInViewModel : ViewModelBase, IHomeLoggedInViewModel, IRecipient<ValueChangedMessage<ShellMessage>>
{
    private readonly ILogger<HomeLoggedInViewModel> _logger;

    [ObservableProperty]
    private bool _isActiveUserControl;

    public HomeLoggedInViewModel(ILogger<HomeLoggedInViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(HomeLoggedInViewModel));

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
