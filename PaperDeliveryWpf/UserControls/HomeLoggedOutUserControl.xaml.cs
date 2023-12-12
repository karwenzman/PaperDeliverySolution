using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls;

public partial class HomeLoggedOutUserControl : UserControl
{
    public HomeLoggedOutUserControl()
    {
        var viewModel = (IHomeLoggedOutViewModel)App.AppHost!.Services.GetService(typeof(IHomeLoggedOutViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(IHomeLoggedOutViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(HomeLoggedOutUserControl)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;
        InitializeComponent();
    }
}
