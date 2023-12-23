using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.Repositories;

namespace PaperDeliveryWpf.ViewModels;

public partial class HomeViewModel : ViewModelBase, IHomeViewModel
{
    [ObservableProperty] private UserModel _userAccount;
    [ObservableProperty] private string _displayMessage;

    private readonly ILogger<HomeViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public HomeViewModel(ILogger<HomeViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(HomeViewModel));

        _userRepository = userRepository;
        UserAccount = _userRepository.GetByUserName(Thread.CurrentPrincipal.Identity.Name);

        DisplayMessage = $"Hello {UserAccount.DisplayName}! You are logged in.";
    }
}
