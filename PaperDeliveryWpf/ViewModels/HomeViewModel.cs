using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.Repositories;
using System.Security.Principal;

namespace PaperDeliveryWpf.ViewModels;

public partial class HomeViewModel : ViewModelBase, IHomeViewModel
{
    [ObservableProperty] private UserModel? _userAccount;
    [ObservableProperty] private string? _displayMessage;
    [ObservableProperty] private string? _principalUserName;
    [ObservableProperty] private string? _principalAuthenticationType;
    [ObservableProperty] private bool _principalIsAuthenticated;
    [ObservableProperty] private bool _principalIsAdmin;
    [ObservableProperty] private bool _principalIsUser;

    private readonly ILogger<HomeViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public HomeViewModel(ILogger<HomeViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(HomeViewModel));

        // This loads the identity of the current OS (in this case Win10)
        //var genericUser = WindowsIdentity.GetCurrent();
        //PrincipalAuthenticationType = genericUser.AuthenticationType;
        //PrincipalIsAuthenticated = genericUser.IsAuthenticated;
        //PrincipalUserName = genericUser.Name;

        var genericAppPrincipal = GetCurrentApplicationPrincipal();
        var genericAppIdentity = GetCurrentApplicationIdentity();

        _userRepository = userRepository;
        UserAccount = _userRepository.GetByUserName(genericAppIdentity.Name);

        // This loads the identity of the current application (in this case PaperDeliverySolution)
        PrincipalAuthenticationType = genericAppIdentity.AuthenticationType;
        PrincipalIsAuthenticated = genericAppIdentity.IsAuthenticated;
        //PrincipalUserName = GetCurrentApplicationIdentity().Name;
        PrincipalUserName = genericAppIdentity.Name;
        PrincipalIsAdmin = genericAppPrincipal.IsInRole("admin");
        PrincipalIsUser = genericAppPrincipal.IsInRole("user");
        
        DisplayMessage = $"Hello {UserAccount.DisplayName}! You are logged in.";
    }
}
