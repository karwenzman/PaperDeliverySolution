using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.Views;

public partial class ShellFooterView : UserControl
{
    public ShellFooterView()
    {
        var viewModel = (IShellFooterViewModel)App.AppHost!.Services.GetService(typeof(IShellFooterViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(IShellFooterViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(ShellFooterView)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;

        InitializeComponent();
    }
}
