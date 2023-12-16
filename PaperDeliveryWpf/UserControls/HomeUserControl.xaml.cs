using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls;

public partial class HomeUserControl : UserControl
{
    public HomeUserControl()
    {
        var viewModel = (IHomeViewModel)App.AppHost!.Services.GetService(typeof(IHomeViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(IHomeViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(HomeUserControl)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;
        InitializeComponent();
    }
}
