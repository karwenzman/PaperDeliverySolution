using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Messages;
using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;
using PaperDeliveryWpf.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Principal;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    private readonly IUserRepository _userRepository;
    private UserModel? _user = new();

    private string _uiLogin = string.Empty;
    private string _uiPassword = string.Empty;

    [Required(ErrorMessage = "Enter your login!")]
    public string UiLogin
    {
        get => _uiLogin;
        set
        {
            if (SetProperty(ref _uiLogin, value, true))
            {
                LoginButtonCommand.NotifyCanExecuteChanged();
            }
        }
    }

    [Required(ErrorMessage = "Enter your password!")]
    [MinLength(3, ErrorMessage = "You need to enter minimum 3 charaters.")]
    public string UiPassword
    {
        get => _uiPassword;
        set
        {
            if (SetProperty(ref _uiPassword, value, true))
            {
                LoginButtonCommand.NotifyCanExecuteChanged();
            }
        }
    }

    [ObservableProperty] private IDatabaseOptions _databaseOptions;
    [ObservableProperty] private ApplicationOptions _applicationOptions;

    private readonly ILogger<LoginViewModel> _logger;
    private readonly IServiceProvider _serviceProvider;

    public LoginViewModel(ILogger<LoginViewModel> logger, IServiceProvider serviceProvider, IOptions<DatabaseOptionsUsingAccess> databaseOptions, IOptions<ApplicationOptions> applicationOptions)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoginViewModel));

        _serviceProvider = serviceProvider;
        _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();

        DatabaseOptions = databaseOptions.Value;
        ApplicationOptions = applicationOptions.Value;

        // TODO - How to close this UserControl?
    }

    #region ***** RelayCommand *****
    [RelayCommand(CanExecute = nameof(CanLoginButton))]
    public void LoginButton()
    {
        bool validUser = _userRepository.Authenticate(new NetworkCredential(UiLogin, UiPassword));

        if (validUser)
        {
            // here might be a change in logic needed; adding roles
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UiLogin), null);
            _user = _userRepository.GetByUserName(UiLogin);
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(_user!));
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.HomeUserControl }));
            _logger.LogInformation("** User {user} has logged in.", UiLogin);
        }
        else
        {
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(new UserModel()));
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.None }));
            _logger.LogInformation("** User authentication failed on account {user}.", UiLogin);
        }

    }
    public bool CanLoginButton()
    {
        bool output = true;

        if (string.IsNullOrWhiteSpace(UiLogin))
        {
            output = false;
        }
        if (string.IsNullOrWhiteSpace(UiPassword))
        {
            output = false;
        }
        if (HasErrors)
        {
            output = false;
        }

        return output;
    }


    [RelayCommand]
    public void CancelButton()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<UserModel>(new UserModel()));
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ShellMessage>(new ShellMessage { SetToActive = ActivateVisibility.StartUserControl }));
    }
    #endregion ***** End Of RelayCommand *****
}
