using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.Views;

public partial class ShellHeaderView : UserControl
{
    public ShellHeaderView()
    {
        var viewModel = (IShellHeaderViewModel)App.AppHost!.Services.GetService(typeof(IShellHeaderViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(IShellHeaderViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(ShellHeaderView)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;
        InitializeComponent();
    }
}
