using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls;

public partial class LogoutUserControl : UserControl
{
    public LogoutUserControl()
    {
        var viewModel = (ILogoutViewModel)App.AppHost!.Services.GetService(typeof(ILogoutViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(ILogoutViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(LogoutUserControl)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;
        InitializeComponent();
    }
}
