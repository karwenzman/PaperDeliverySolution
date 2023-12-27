using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.Repositories;

namespace PaperDeliveryWpf.ViewModels;

public partial class AccountViewModel : ViewModelBase, IAccountViewModel
{
    [ObservableProperty]
    private UserModel? _currentUser = new();

    private readonly ILogger<AccountViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public AccountViewModel(ILogger<AccountViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;

        if (!IsUserInRole("user"))
        {
            _logger.LogError("** Access denied on page {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"Access denied on page {nameof(AccountViewModel)} by {GetUserName()}!");
        }

        CurrentUser = _userRepository.GetByUserName(GetUserName());
        if (CurrentUser == null)
        {
            _logger.LogError("** No valid user account is loaded while accessing {class} by {name}!", nameof(AccountViewModel), GetUserName());
            throw new ArgumentException($"No valid user account is loaded while accessing class {nameof(AccountViewModel)} by {GetUserName()}!");
        }

        _logger.LogInformation("* Loading {class}", nameof(AccountViewModel));
    }
}
