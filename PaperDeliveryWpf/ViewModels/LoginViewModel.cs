﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryLibrary.Models;
using PaperDeliveryWpf.Repositories;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoginViewModel : ViewModelBase, ILoginViewModel
{
    // Constructor injection.
    private readonly ILogger<LoginViewModel> _logger;
    private readonly IServiceProvider _serviceProvider;

    // Private fields to store the loaded services.
    private readonly IUserRepository _userRepository;

    // Properties using CommunityToolkit.
    [ObservableProperty]
    private string _showSomething;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _uiLoginName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _uiPassword;



    public LoginViewModel(ILogger<LoginViewModel> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoginViewModel));

        _serviceProvider = serviceProvider;
        _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();

        ShowSomething = "Hallo Welt!";
        UiLoginName = string.Empty;
        UiPassword = string.Empty;
    }

    // RelayCommands using the CommunityToolkit.
    [RelayCommand(CanExecute = nameof(CanLogin))]
    public void Login()
    {
        // TODO - Error handling.
        UserModel? userModel = _userRepository.Login(UiLoginName, UiPassword);
        ShowSomething = userModel!.DisplayName;

        _logger.LogInformation("** User {user} has logged in.", userModel.Email);
    }
    public bool CanLogin()
    {
        // TODO - How to enable this, if a character is entered into TextBox?
        return !string.IsNullOrEmpty(UiLoginName) && !string.IsNullOrEmpty(UiPassword);
    }


}