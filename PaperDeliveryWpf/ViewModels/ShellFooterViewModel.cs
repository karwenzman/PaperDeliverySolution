using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Models;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellFooterViewModel : ViewModelBase, IShellFooterViewModel, IRecipient<ValueChangedMessage<UserModel>>
{
    // Constructor injection.
    private readonly ILogger<ShellFooterViewModel> _logger;

    // Properties using CommunityToolkit.
    [ObservableProperty]
    private string _loginName;


    public ShellFooterViewModel(ILogger<ShellFooterViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellFooterViewModel));

        WeakReferenceMessenger.Default.Register(this);
        LoginName = "n/a";
    }

    public void Receive(ValueChangedMessage<UserModel> message)
    {
        LoginName = message.Value.Email;
    }
}
