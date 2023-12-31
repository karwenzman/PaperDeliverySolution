﻿using CommunityToolkit.Mvvm.ComponentModel;
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
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellViewModel : ViewModelBase,
    IShellViewModel,
    IRecipient<ValueChangedMessage<ShellMessage>>
{
    [ObservableProperty]
    private object? _currentViewModel = new();

    [ObservableProperty]
    private UserModel? _currentUser = new();

    [ObservableProperty]
    private string _applicationHomeDirectory = string.Empty;

    [ObservableProperty]
    private string _applicationName = string.Empty;

    [ObservableProperty]
    private string _applicationVersion = string.Empty;

    [ObservableProperty]
    private bool _isActiveLoginMenuItem = true;

    [ObservableProperty]
    private bool _isActiveLogoutMenuItem = true;

    [ObservableProperty]
    private bool _isActiveUserMenuItem = true;

    [ObservableProperty]
    private bool _isActiveAdminMenuItem = true;

    private readonly ILogger<ShellViewModel> _logger;
    private readonly IOptions<ApplicationOptions> _options;
    private readonly IUserRepository _userRepository;

    public ShellViewModel(ILogger<ShellViewModel> logger,
        IOptions<ApplicationOptions> options,
        IUserRepository userRepository)
    {
        _logger = logger;
        _options = options;
        _userRepository = userRepository;

        _logger.LogInformation("* Loading {class}", nameof(ShellViewModel));

        ApplicationName = _options.Value.ApplicationName;
        ApplicationHomeDirectory = _options.Value.ApplicationHomeDirectory;
        ApplicationVersion = GetApplicationVersion();

        ManageUserControls(new ShellMessage { SetToActive = LoadViewModel.LoginUserControl });

        StopCommand = new CommandBinding(ApplicationCommands.Stop, Stop, CanStop);

        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    #region ***** Event *****
    public void ShellView_Closing(object? sender, CancelEventArgs e)
    {
        MessageBoxResult messageBoxResult = MessageBox.Show(
            "Soll das Fenster geschlossen werden?",
            $"{nameof(ShellView_Closing)}",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question,
            MessageBoxResult.No);

        if (messageBoxResult == MessageBoxResult.No)
        {
            e.Cancel = true;
        }
    }
    #endregion ***** End Of Event *****

    #region ***** CommandBinding *****
    public CommandBinding StopCommand { get; set; }
    private void Stop(object sender, ExecutedRoutedEventArgs e)
    {
        Application.Current.MainWindow.Close();
    }
    private void CanStop(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    #endregion ***** End OF CommandBinding *****

    #region ***** RelayCommand *****
    [RelayCommand]
    public void LoginMenuItem()
    {
        ManageUserControls(new ShellMessage { SetToActive = LoadViewModel.LoginUserControl });
    }

    [RelayCommand]
    public void LogoutMenuItem()
    {
        ManageUserControls(new ShellMessage { SetToActive = LoadViewModel.LogoutUserControl });
    }

    [RelayCommand]
    public void AccountMenuItem()
    {
        try
        {
            ManageUserControls(new ShellMessage { SetToActive = LoadViewModel.AccountUserControl });
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            string caption = nameof(AccountMenuItem);

            // TODO - MessageBoxes should not be handled by the ViewModel.
            MessageBoxResult messageBoxResult = MessageBox.Show(
                messageBoxText: message,
                caption: caption,
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);

            ManageUserControls(new ShellMessage { SetToActive = LoadViewModel.HomeUserControl });
        }
    }

    [RelayCommand]
    public void AccountManagerMenuItem()
    {
        try
        {
            ManageUserControls(new ShellMessage { SetToActive = LoadViewModel.AccountManagerUserControl });
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            string caption = nameof(AccountManagerMenuItem);

            // TODO - MessageBoxes should not be handled by the ViewModel.
            MessageBoxResult messageBoxResult = MessageBox.Show(
                messageBoxText: message,
                caption: caption,
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);

            ManageUserControls(new ShellMessage { SetToActive = LoadViewModel.HomeUserControl });
        }
    }
    #endregion ***** End OF RelayCommand *****

    private void ManageUserControls(ShellMessage message)
    {
        IsActiveLoginMenuItem = message.SetToActive == LoadViewModel.StartUserControl || message.SetToActive == LoadViewModel.ErrorUserControl;
        IsActiveLogoutMenuItem = IsUserAuthenticated();
        IsActiveUserMenuItem = IsUserAuthenticated() && IsUserInRole("user");
        IsActiveAdminMenuItem = IsUserAuthenticated() && IsUserInRole("admin");

        switch (message.SetToActive)
        {
            case LoadViewModel.ErrorUserControl:
                CurrentUser = new();
                CurrentViewModel = App.AppHost!.Services.GetRequiredService<IErrorViewModel>();
                break;
            case LoadViewModel.LoginUserControl:
                IsActiveLoginMenuItem = false;
                IsActiveLogoutMenuItem = false;
                IsActiveUserMenuItem = false;
                IsActiveAdminMenuItem = false;
                DisposeThreadPrincipal();
                CurrentUser = new();
                CurrentViewModel = App.AppHost!.Services.GetRequiredService<ILoginViewModel>();
                break;
            case LoadViewModel.LogoutUserControl:
                CurrentViewModel = App.AppHost!.Services.GetRequiredService<ILogoutViewModel>();
                break;
            case LoadViewModel.HomeUserControl:
                CurrentUser = _userRepository.GetByUserName(GetUserName());
                CurrentViewModel = App.AppHost!.Services.GetRequiredService<IHomeViewModel>();
                break;
            case LoadViewModel.StartUserControl:
                CurrentUser = new();
                CurrentViewModel = App.AppHost!.Services.GetRequiredService<IStartViewModel>();
                break;
            case LoadViewModel.AccountUserControl:
                IsActiveLoginMenuItem = false;
                IsActiveLogoutMenuItem = false;
                IsActiveUserMenuItem = false;
                IsActiveAdminMenuItem = false;
                CurrentUser = _userRepository.GetByUserName(GetUserName());
                CurrentViewModel = App.AppHost!.Services.GetRequiredService<IAccountViewModel>();
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<AccountMessage>(new AccountMessage { Account = _userRepository.GetByUserName(GetUserName()), SetAccountUserControl = SetAccountUserControl.Default }));
                break;
            case LoadViewModel.AccountManagerUserControl:
                IsActiveLoginMenuItem = false;
                IsActiveLogoutMenuItem = false;
                IsActiveUserMenuItem = false;
                IsActiveAdminMenuItem = false;
                CurrentUser = _userRepository.GetByUserName(GetUserName());
                CurrentViewModel = App.AppHost!.Services.GetRequiredService<IAccountManagerViewModel>();
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<AccountManagerMessage>(new AccountManagerMessage { Account = _userRepository.GetByUserName(GetUserName()) }));
                break;
        }
    }

    private static string GetApplicationVersion()
    {
        // TODO - Work on Deployment
        //if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
        //{
        //    return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.
        //        CurrentVersion.ToString();
        //}
        //return "Not network deployed";

        // This is the assembly version only. See projet's properties. 
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        return $"{version!.Major}.{version!.Minor}.{version!.Build}.{version!.Revision}";
    }

    /// <summary>
    /// This method is listening to the <see cref="WeakReferenceMessenger"/>.
    /// </summary>
    /// <param name="message">This parameter contains the information, which view is the <see cref="CurrentViewModel"/>.</param>
    public void Receive(ValueChangedMessage<ShellMessage> message)
    {
        ManageUserControls(message.Value);
    }
}
