using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls;

public partial class StartUserControl : UserControl
{
    public StartUserControl()
    {
        var viewModel = (IStartViewModel)App.AppHost!.Services.GetService(typeof(IStartViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(IStartViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(StartUserControl)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;
        InitializeComponent();
    }
}
