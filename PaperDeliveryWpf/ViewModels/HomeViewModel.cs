using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;
using PaperDeliveryWpf.Repositories;

namespace PaperDeliveryWpf.ViewModels;

public partial class HomeViewModel : ViewModelBase, IHomeViewModel
{
    [ObservableProperty] private UserModel _userAccount;
    [ObservableProperty] private string _displayMessage;

    private readonly ILogger<HomeViewModel> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IOptions<DatabaseOptionsUsingAccess> _databaseOptions;

    public HomeViewModel(ILogger<HomeViewModel> logger, IUserRepository userRepository, IOptions<DatabaseOptionsUsingAccess> databaseOptions)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(HomeViewModel));

        _userRepository = userRepository;
        _databaseOptions = databaseOptions;
        UserAccount = _userRepository.GetUserByUserName(Thread.CurrentPrincipal.Identity.Name, _databaseOptions.Value);

        DisplayMessage = $"Hello {UserAccount.DisplayName}! You are logged in.";
    }
}
