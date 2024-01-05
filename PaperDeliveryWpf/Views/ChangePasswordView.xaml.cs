using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace PaperDeliveryWpf.Views;

public partial class ChangePasswordView : Window
{
    private readonly IChangePasswordViewModel _viewModel;

    private readonly ILogger<ChangePasswordView> _logger;

    public ChangePasswordView(ILogger<ChangePasswordView> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ChangePasswordView));

        _viewModel = App.AppHost!.Services.GetRequiredService<IChangePasswordViewModel>();
        DataContext = _viewModel;

        InitializeComponent();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}
