using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.Repositories;
using System.Diagnostics;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    private readonly IUserRepository _userRepository;
    private UserModel? _user = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor((nameof(LoginButtonCommand)))]
    private string? _uiLogin = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginButtonCommand))]
    private string? _uiPassword = string.Empty;

    private readonly ILogger<LoginViewModel> _logger;
    private readonly IServiceProvider _serviceProvider;

    public LoginViewModel(ILogger<LoginViewModel> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoginViewModel));

        _serviceProvider = serviceProvider;
        _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();

        // TODO - How to close this UserControl?
    }

    #region ***** RelayCommand *****
    [RelayCommand(CanExecute = nameof(CanLoginButton))]
    public void LoginButton()
    {
        _user = _userRepository.Login(UiLogin!, UiPassword!);

        if (_user == null)
        {
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(new UserModel()));
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.None }));
            _logger.LogInformation("** User authentication failed on account {user}.", UiLogin);
        }
        else if (_user.Id == 0)
        {
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(new UserModel()));
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.None }));
            _logger.LogInformation("** User authentication failed on account {user}.", UiLogin);
        }
        else
        {
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(_user));
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.HomeUserControl }));
            _logger.LogInformation("** User {user} has logged in.", UiLogin);
        }
    }
    public bool CanLoginButton()
    {
        // TODO - How to enable this, if a character is entered into TextBox?
        // How to enable to move to next textbox when hitting enter or tab?
        bool output = true;

        //if (string.IsNullOrWhiteSpace(UiLogin) && string.IsNullOrWhiteSpace(UiPassword))
        //{
        //    output = false;
        //}

        if (string.IsNullOrWhiteSpace(UiLogin))
        {
            output = false;
        }
        if (string.IsNullOrWhiteSpace(UiPassword))
        {
            output = false;
        }

        //bool output = false;
        //if (UiLogin.Length > 0 && UiPassword.Length > 0)
        //{
        //    output = true;
        //}

        return output;
    }
    partial void OnUiLoginChanging(string? value)
    {
        Debug.WriteLine($"Passing {nameof(OnUiLoginChanging)}");
        LoginButtonCommand.NotifyCanExecuteChanged();
    }
    partial void OnUiPasswordChanging(string? value)
    {
        Debug.WriteLine($"Passing {nameof(OnUiPasswordChanging)}");
        LoginButtonCommand.NotifyCanExecuteChanged();
    }
    partial void OnUiLoginChanged(string? value)
    {
        Debug.WriteLine($"Passing {nameof(OnUiLoginChanged)}");
        LoginButtonCommand.NotifyCanExecuteChanged();
    }
    partial void OnUiPasswordChanged(string? value)
    {
        Debug.WriteLine($"Passing {nameof(OnUiPasswordChanged)}");
        LoginButtonCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    public void CancelButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(new UserModel()));
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.StartUserControl }));
    }
    #endregion ***** End Of RelayCommand *****
}
