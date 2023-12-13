using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.Repositories;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    // Constructor injection.
    private readonly ILogger<LoginViewModel> _logger;
    private readonly IServiceProvider _serviceProvider;

    // Private fields to store the loaded services.
    private readonly IUserRepository _userRepository;
    private ShellMessage _message = new();

    // Properties using CommunityToolkit.
    [ObservableProperty]
    private string _showSomething;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _uiLoginName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _uiPassword;

    [ObservableProperty]
    private bool _isActiveUserControl;

    public LoginViewModel(ILogger<LoginViewModel> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoginViewModel));

        _serviceProvider = serviceProvider;
        _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();

        IsActiveUserControl = true;

        _message = new ShellMessage
        {
            DisplayLoggedIn = true,
            DisplayLoggedOut = false,
            DisplayLogin = false,
        };

        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(_message));

        ShowSomething = "Hallo Welt!";
        UiLoginName = string.Empty;
        UiPassword = string.Empty;
    }

    // RelayCommands using the CommunityToolkit.
    [RelayCommand(CanExecute = nameof(CanLogin))]
    public void Login()
    {
        // TODO - Error handling.
        UserModel? userModel = _userRepository.Login(UiLoginName, UiPassword);
        ShowSomething = userModel!.DisplayName;

        // App wide communication. Recepients are listening for this message.
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(userModel));

        _message = new ShellMessage
        {
            DisplayLoggedIn = true,
            DisplayLoggedOut = false,
            DisplayLogin = false,
        };

        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(_message));

        _logger.LogInformation("** User {user} has logged in.", userModel.Email);
    }
    public bool CanLogin()
    {
        // TODO - How to enable this, if a character is entered into TextBox?
        return !string.IsNullOrEmpty(UiLoginName) && !string.IsNullOrEmpty(UiPassword);
    }
}
