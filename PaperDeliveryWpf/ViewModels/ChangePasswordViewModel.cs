using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using PaperDeliveryWpf.Repositories;
using System.Security;

namespace PaperDeliveryWpf.ViewModels;

public partial class ChangePasswordViewModel : ViewModelBase, IChangePasswordViewModel
{
    [ObservableProperty]
    private SecureString? _currentPassword;
    [ObservableProperty]
    private SecureString? _newPassword;
    [ObservableProperty]
    private SecureString? _confirmPassword;

    [ObservableProperty] private bool _isVisibleSubmitNewPasswordButton = true;
    [ObservableProperty] private bool _isVisibleConfirmOldPasswordButton = true;
    [ObservableProperty] private bool _isEnabledCurrentPassword = true;
    [ObservableProperty] private bool _isEnabledNewPassword = true;
    [ObservableProperty] private bool _isEnabledConfirmPassword = true;

    private readonly ILogger<ChangePasswordViewModel> _logger;
    private readonly IUserRepository _userRepository;

    public ChangePasswordViewModel(ILogger<ChangePasswordViewModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;

        _logger.LogInformation("* Loading {class}", nameof(ChangePasswordViewModel));

        WeakReferenceMessenger.Default.RegisterAll(this);

    }

    [RelayCommand]
    public void SubmitNewPasswordButton()
    {

    }

    [RelayCommand]
    public void ConfirmOldPasswordButton()
    {

    }

    [RelayCommand]
    public void CloseButton()
    {

    }
}
