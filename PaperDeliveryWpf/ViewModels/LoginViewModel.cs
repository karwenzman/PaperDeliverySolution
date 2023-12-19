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

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    private readonly IUserRepository _userRepository;
    private UserModel? _user = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor((nameof(LoginButtonCommand)))]
    private string _uiLogin = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginButtonCommand))]
    private string _uiPassword = string.Empty;

    [ObservableProperty]
    private IDatabaseOptions _databaseOptions;

    [ObservableProperty]
    private ApplicationOptions _applicationOptions;

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
