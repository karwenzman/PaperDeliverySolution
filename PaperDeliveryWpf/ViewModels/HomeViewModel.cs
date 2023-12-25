using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public partial class HomeViewModel : ViewModelBase, IHomeViewModel
{
    [ObservableProperty] private string? _displayMessage;
    [ObservableProperty] private string? _principalUserName;
    [ObservableProperty] private string? _principalAuthenticationType;
    [ObservableProperty] private bool _principalIsAuthenticated;
    [ObservableProperty] private bool _principalIsAdmin;
    [ObservableProperty] private bool _principalIsUser;
    [ObservableProperty] private bool _principalIsGuest;

    private readonly ILogger<HomeViewModel> _logger;

    public HomeViewModel(ILogger<HomeViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(HomeViewModel));

        PrincipalIsAuthenticated = IsUserAuthenticated();
        PrincipalUserName = GetUserName();
        PrincipalAuthenticationType = GetUserAuthenticationType();
        PrincipalIsAdmin = IsUserInRole("admin");
        PrincipalIsUser = IsUserInRole("user");
        PrincipalIsGuest = IsUserInRole("guest");

        DisplayMessage = $"Hello {GetUserName()}! You are logged in.";
    }
}
