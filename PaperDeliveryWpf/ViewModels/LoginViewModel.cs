using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    private readonly ILogger<LoginViewModel> _logger;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _showSomething;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _uiLoginName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _uiPassword;



    public LoginViewModel(ILogger<LoginViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoginViewModel));
        
        ShowSomething = "Hallo Welt!";
        UiLoginName = string.Empty;
        UiPassword = string.Empty;


    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    public void Login()
    {
        ShowSomething = "Login simuliert";
    }
    public bool CanLogin()
    {
        return !string.IsNullOrEmpty(UiLoginName) && !string.IsNullOrEmpty(UiPassword);
    }


}
