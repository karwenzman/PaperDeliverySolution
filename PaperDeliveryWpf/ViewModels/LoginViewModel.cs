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

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    private readonly IUserRepository _userRepository;
    private UserModel? _user = new();

    private string _uiLogin = string.Empty;
    private string _uiPassword = string.Empty;

    [Required(ErrorMessage = "Input is mandatory!")]
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

    [Required(ErrorMessage = "Input is mandatory!")]
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
        _user = _userRepository.Login(UiLogin, UiPassword, DatabaseOptions);

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
        bool output = true;

        // TODO - Check, if there a DataError messages, then return false, also
        if (string.IsNullOrWhiteSpace(UiLogin))
        {
            output = false;
        }
        if (string.IsNullOrWhiteSpace(UiPassword))
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
