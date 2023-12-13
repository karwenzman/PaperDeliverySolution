using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls;

public partial class LoggedInUserControl : UserControl
{
    public LoggedInUserControl()
    {
        var viewModel = (ILoggedInViewModel)App.AppHost!.Services.GetService(typeof(ILoggedInViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(ILoggedInViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(LoggedInUserControl)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;
        InitializeComponent();
    }
}
