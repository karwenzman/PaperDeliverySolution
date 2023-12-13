using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls;

public partial class LoggedOutUserControl : UserControl
{
    public LoggedOutUserControl()
    {
        var viewModel = (ILoggedOutViewModel)App.AppHost!.Services.GetService(typeof(ILoggedOutViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(ILoggedOutViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(LoggedOutUserControl)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;
        InitializeComponent();
    }
}
