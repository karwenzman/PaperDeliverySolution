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
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellViewModel : ViewModelBase, IShellViewModel,
    IRecipient<ValueChangedMessage<ShellMessage>>,
    IRecipient<ValueChangedMessage<UserModel>>
{
    [ObservableProperty]
    private object? _currentView;

    [ObservableProperty]
    private ShellMessage? _shellMessage = new();

    [ObservableProperty]
    private UserModel? _userAccount = new();

    [ObservableProperty]
    private string _applicationHomeDirectory = string.Empty;

    [ObservableProperty]
    private string _applicationName = string.Empty;

    [ObservableProperty]
    private string _applicationVersion = string.Empty;

    [ObservableProperty]
    private string _userEmail = string.Empty;

    [ObservableProperty]
    private string _userName = string.Empty;

    [ObservableProperty]
    private bool _isActiveLoginMenuItem = true;

    [ObservableProperty]
    private bool _isActiveLogoutMenuItem = true;

    [ObservableProperty]
    private bool _isActiveUserMenuItem = true;

    private readonly ILogger<ShellViewModel> _logger;
    private readonly IOptions<ApplicationOptions> _options;
    private readonly IServiceProvider _serviceProvider;

    public ShellViewModel(ILogger<ShellViewModel> logger, IOptions<ApplicationOptions> options, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellViewModel));

        _options = options;
        ApplicationName = _options.Value.ApplicationName;
        ApplicationHomeDirectory = _options.Value.ApplicationHomeDirectory;
        ApplicationVersion = GetApplicationVersion();

        _serviceProvider = serviceProvider; // Check, if needed!

        ManageUserControls(new ShellMessage { SetToActive = ActivateVisibility.StartUserControl });

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
    [RelayCommand(CanExecute = nameof(CanLoginMenuItem))]
    public void LoginMenuItem()
    {
        ManageUserControls(new ShellMessage { SetToActive = ActivateVisibility.LoginUserControl });
    }
    public bool CanLoginMenuItem()
    {
        return true;
    }

    [RelayCommand(CanExecute = nameof(CanLogoutMenuItem))]
    public void LogoutMenuItem()
    {
        ManageUserControls(new ShellMessage { SetToActive = ActivateVisibility.StartUserControl });

        ManageUserAccount(UserAccount = null);
    }
    public bool CanLogoutMenuItem()
    {
        return true;
    }

    [RelayCommand(CanExecute = nameof(CanUserMenuItem))]
    public void UserMenuItem()
    {

    }
    public bool CanUserMenuItem()
    {
        return true;
    }

    #endregion ***** End OF RelayCommand *****

    private void ManageUserControls(ShellMessage? message)
    {
        if (message == null)
        {
            ManageUserControls(new ShellMessage { SetToActive = ActivateVisibility.None });
        }
        else
        {
            switch (message.SetToActive)
            {
                case ActivateVisibility.None:
                    IsActiveLoginMenuItem = false;
                    IsActiveLogoutMenuItem = false;
                    IsActiveUserMenuItem = false;
                    _logger.LogCritical("ActivateVisibility was set to None in {class}", nameof(ShellViewModel));
                    CurrentView = App.AppHost!.Services.GetRequiredService<IErrorViewModel>();
                    break;
                case ActivateVisibility.LoginUserControl:
                    IsActiveLoginMenuItem = false;
                    IsActiveLogoutMenuItem = false;
                    IsActiveUserMenuItem = false;
                    CurrentView = App.AppHost!.Services.GetRequiredService<ILoginViewModel>();
                    break;
                case ActivateVisibility.LogoutUserControl:
                    IsActiveLoginMenuItem = false;
                    IsActiveLogoutMenuItem = false;
                    IsActiveUserMenuItem = false;
                    CurrentView = App.AppHost!.Services.GetRequiredService<ILogoutViewModel>();
                    break;
                case ActivateVisibility.HomeUserControl:
                    IsActiveLoginMenuItem = false;
                    IsActiveLogoutMenuItem = true;
                    IsActiveUserMenuItem = true;
                    CurrentView = App.AppHost!.Services.GetRequiredService<IHomeViewModel>();
                    break;
                case ActivateVisibility.StartUserControl:
                    IsActiveLoginMenuItem = true;
                    IsActiveLogoutMenuItem = false;
                    IsActiveUserMenuItem = false;
                    CurrentView = App.AppHost!.Services.GetRequiredService<IStartViewModel>();
                    break;
            }
        }
    }

    private void ManageUserAccount(UserModel? messge)
    {
        if (messge == null)
        {
            UserEmail = string.Empty;
            UserName = string.Empty;
        }
        else
        {
            UserEmail = messge.Email;
            UserName = messge.Name;
        }
    }

    private string GetApplicationVersion()
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

    public void Receive(ValueChangedMessage<ShellMessage> message)
    {
        ManageUserControls(message.Value);
    }

    public void Receive(ValueChangedMessage<UserModel> message)
    {
        ManageUserAccount(message.Value);
    }
}
