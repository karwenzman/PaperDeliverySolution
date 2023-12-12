using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls;

public partial class HomeLoggedInUserControl : UserControl
{
    public HomeLoggedInUserControl()
    {
        var viewModel = (IHomeLoggedInViewModel)App.AppHost!.Services.GetService(typeof(IHomeLoggedInViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(IHomeLoggedInViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(HomeLoggedInUserControl)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;
        InitializeComponent();
    }
}
